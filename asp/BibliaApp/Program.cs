using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BibliaApp
{
    partial class Program
    {
        const string ConnectionString = "Data Source=CMVWR72;Initial Catalog=BibleDb;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        static void Main(string[] args)
        {
            //MoveBooks();

            //MoveDataFromLocalToAzure();
            SetNextPrev();
        }


        public static void SetNextPrev()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {
                var allBooks = dbContext.BooksExtended.ToList();
                allBooks = allBooks.OrderBy(x => x.BookGlobalNumber).ToList();
                for (int i = 1; i < allBooks.Count; i++)
                {
                    allBooks[i].PreviousBookGuid = allBooks[i - 1].Guid;
                    allBooks[i-1].NextBookGuid = allBooks[i].Guid;
                    
                    if (i%20 == 0)
                        dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }
        }


        private static void MoveBooks()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {
                dbContext.Books.RemoveRange(dbContext.Books.ToList());

                dbContext.SaveChanges();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {

                using (ApplicationDbContext dbContext2 = new ApplicationDbContext())
                {
                    var books = dbContext2.Books.ToList();
                    foreach (var bookDomainObject in books)
                    {
                        dbContext.Books.Add(bookDomainObject);
                        dbContext.SaveChanges();
                    }

                }

            }
        }


        private static void MoveDataFromLocalToAzure()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {
                dbContext.Comments.RemoveRange(dbContext.Comments.ToList());
                dbContext.BooksExtended.RemoveRange(dbContext.BooksExtended.ToList());

                dbContext.SaveChanges();
            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {

                using (ApplicationDbContext dbContext2 = new ApplicationDbContext())
                {
                    var booksExted = dbContext2.BooksExtended.ToList();
                    foreach (var bookExtendedDomainObject in booksExted)
                    {
                        dbContext.BooksExtended.Add(bookExtendedDomainObject);
                        dbContext.SaveChanges();                  
                    }

                    dbContext.Comments.AddRange(dbContext2.Comments.ToList());
                    dbContext.SaveChanges();
                }

            }
        }

       // "[NV#085] Czy można wierzyć w sny, noc poślubna (Q&A#11)"

        private static void moveCommentsFromAzure()
        {
            var commentsFromAzure = GetCommentsFromAzure();

            foreach (var commentDomainObject in commentsFromAzure)
            {
                BookExtendedDomainObject book;
                BookExtendedDomainObject bookFromLocalDb;
                using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
                {
                    book = dbContext.BooksExtended.Where(x => x.Guid == commentDomainObject.BookGuid).FirstOrDefault();
                }
                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    bookFromLocalDb = dbContext.BooksExtended.Where(x => x.BookName == book.BookName).FirstOrDefault();
                }
                if (bookFromLocalDb != null)
                {
                    commentDomainObject.BookGuid = bookFromLocalDb.Guid;
                }
                else
                {
                    commentDomainObject.BookGuid = Guid.Empty;
                }

            }

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var confirmedComments = commentsFromAzure.Where(x => x.BookGuid != Guid.Empty).ToList();
                dbContext.Comments.AddRange(confirmedComments);
                dbContext.SaveChanges();
            }
        }

        private static List<CommentDomainObject> GetCommentsFromAzure()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext(ApplicationDbContext.ConnectionStringAzure))
            {
                return dbContext.Comments.ToList();
            }
        }


        private static List<BookExtendedDomainObject> CreateBookExtended()
        {
            var listExtended = new List<BookExtendedDomainObject>();
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var books = dbContext.Books.ToList();

                foreach (var book in books)
                {
                    BookExtendedDomainObject bookExtended = new BookExtendedDomainObject();

                    bookExtended.Passages = dbContext.Passages.Where(x => x.BookGuid == book.Guid).ToList();
                    bookExtended.BookName = book.BookName;
                    bookExtended.BookFullName = book.BookFullName;
                    bookExtended.Guid = book.Guid;
                    bookExtended.PreviousBookGuid = listExtended.LastOrDefault()?.Guid ?? Guid.Empty;


                    var bookExtendedDomainObject = listExtended.LastOrDefault();
                    if (bookExtendedDomainObject != null)
                        bookExtendedDomainObject.NextBookGuid = bookExtended.Guid;


                    bookExtended.BookGlobalNumber = book.BookGlobalNumber;

                    bookExtended.BeforeSave();
                    listExtended.Add(bookExtended);
                }
                dbContext.BooksExtended.AddRange(listExtended);
                dbContext.SaveChanges();

            }
            return listExtended;
        }

        private static List<PassageDomainObject> GetPassages()
        {
            string url = @"C:\Users\Łukasz\Desktop\Biblia Tysiąclecia - wydanie II poprawione.txt";
            var text = System.IO.File.ReadAllLines(url);

            var passages = new List<PassageDomainObject>();
            var books = new List<BookDomainObject>();
            BookDomainObject currentBook = null;
            bool splitByFour = false;
            int i = 0;
            foreach (var passageString in text)
            {
                //nowa ksiega
                if (string.IsNullOrEmpty(passageString))
                {
                    currentBook = null;
                    splitByFour = false;
                    continue;
                }

                var splitedPassage = passageString.Split();
                var passagesList = splitedPassage.ToList();
                passagesList.RemoveAll(x => x == "");

                if (currentBook == null)
                {
                    int str;
                    currentBook = new BookDomainObject();
                    books.Add(currentBook);
                    currentBook.Guid = Guid.NewGuid().GenerateComb();
                    currentBook.BookGlobalNumber = i++;

                    if (passageString.Contains("Pieśń nad Pieśniami"))
                    {
                        splitByFour = true;
                        currentBook.BookName = GetBooks().Where(x => x.BookFullName.Contains(splitedPassage[0] + " " + splitedPassage[1])).FirstOrDefault().BookName + " " + splitedPassage[2];
                        currentBook.BookFullName = splitedPassage[0] + " " + splitedPassage[1] + " " + splitedPassage[2];// + " " + splitedPassage[3];
                        currentBook.SubbookNumber = Int32.Parse(splitedPassage[3]);
                        passagesList.RemoveAt(0);
                    }
                    else if (passageString.Contains("Powtórzonego Prawa"))
                    {
                        splitByFour = true;
                        currentBook.BookName =
                            GetBooks()
                                .Where(x => x.BookFullName.Contains(splitedPassage[0] + " " + splitedPassage[1]))
                                .FirstOrDefault()
                                .BookName + " " + splitedPassage[2];
                        currentBook.BookFullName = splitedPassage[0] + " " + splitedPassage[1];// + " " + splitedPassage[2];
                        currentBook.SubbookNumber = Int32.Parse(splitedPassage[2]);
                    }
                    else if (Int32.TryParse(splitedPassage[0], out str))
                    {
                        splitByFour = true;
                        currentBook.BookName =
                            GetBooks()
                                .Where(x => x.BookFullName.Contains(splitedPassage[0] + " " + splitedPassage[1]))
                                .FirstOrDefault()
                                .BookName + " " + splitedPassage[2];
                        currentBook.BookFullName = splitedPassage[0] + " " + splitedPassage[1];// + " " + splitedPassage[2];
                        currentBook.SubbookNumber = Int32.Parse(splitedPassage[2]);
                    }
                    else
                    {
                        splitByFour = false;
                        currentBook.BookName =
                            GetBooks().Where(x => x.BookFullName.Contains(splitedPassage[0])).FirstOrDefault().BookName + " " + splitedPassage[1];
                        currentBook.BookFullName = splitedPassage[0];
                        currentBook.SubbookNumber = Int32.Parse(splitedPassage[1]);
                    }
                }


                passagesList.RemoveAt(0);
                passagesList.RemoveAt(0);

                if (splitByFour)
                    passagesList.RemoveAt(0);


                var passage = new PassageDomainObject();
                passage.PassageNumber = Int32.Parse(passagesList[0]);

                passagesList.RemoveAt(0);

                passage.PassageText = string.Join(" ", passagesList);

                passage.BookGuid = currentBook.Guid;
                passage.Book = currentBook.BookName;
                passages.Add(passage);
            }

            passages.ForEach(x => x.Guid = Guid.Empty.GenerateComb());

            var bookExtended = new BookExtendedDomainObject();
            bookExtended.Passages = passages;

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                dbContext.Books.AddRange(books);
                dbContext.Passages.AddRange(passages);
                dbContext.SaveChanges();
            }

            return passages;
        }



        private static List<BookDomainObject> GetBooks()
        {

            List<BookDomainObject> bookDomainObjects = new List<BookDomainObject>();

            string[] books =
            {
                "01 Rdz: Rodzaju                        ",
                "02 Wj: Wyjścia                         ",
                "03 Kpł: Kapłańska                      ",
                "04 Lb: Liczb                           ",
                "05 Pwt: Powtórzonego Prawa             ",
                "06 Joz: Jozuego                        ",
                "07 Sdz: Sędziów                        ",
                "08 Rt: Rut                             ",
                "09 1Sm: 1 Samuela                      ",
                "10 2Sm: 2 Samuela                      ",
                "11 1Krl: 1 Królów                      ",
                "12 2Krl: 2 Królów                      ",
                "13 1Krn: 1 Kronik                      ",
                "14 2Krn: 2 Kronik                      ",
                "15 Ezd: Ezdrasza                       ",
                "16 Ne: Nehemiasza                      ",
                "17 Est: Estery                         ",
                "18 Jo: Joba                            ",
                "19 Ps: Psalmy                          ",
                "20 Prz: Przysłów                       ",
                "21 Kzn: Kaznodziei / Koheleta /        ",
                "22 Pnp: Pieśń nad Pieśniami            ",
                "23 Iz: Izajasza                        ",
                "24 Jr: Jeremiasza                      ",
                "25 Lm: Lamentacje                      ",
                "26 Ez: Ezechiela                       ",
                "27 Dn: Daniela                         ",
                "28 Oz: Ozeasza                         ",
                "29 Jl: Joela                           ",
                "30 Am: Amosa                           ",
                "31 Ab: Abdiasza                        ",
                "32 Jon: Jonasza                        ",
                "33 Mi: Micheasza                       ",
                "34 Na: Nahuma                          ",
                "35 Ha: Habakuka                        ",
                "36 So: Sofoniasza                      ",
                "37 Ag: Aggeusza                        ",
                "38 Za: Zachariasza                     ",
                "39 Ml: Malachiasza                     ",

                "01 Mt: Mateusza                        ",
                "02 Mk: Marka                           ",
                "03 Łk: Łukasza                         ",
                "04 J: Jana                             ",
                "05 Dz: Dzieje Apostolskie              ",
                "06 Rz: Rzymian                         ",
                "07 1Kor: 1 Koryntian                   ",
                "08 2Kor: 2 Koryntian                   ",
                "09 Ga: Galatów                         ",
                "10 Ef: Efezjan                         ",
                "11 Flp: Filipian                       ",
                "12 Kol: Kolosan                        ",
                "13 1Tes: 1 Tesaloniczan                ",
                "14 2Tes: 2 Tesaloniczan                ",
                "15 1Tm: 1 Tymoteusza                   ",
                "16 2Tm: 2 Tymoteusza                   ",
                "17 Tt: Tytusa                          ",
                "18 Flm: Filemona                       ",
                "19 Hbr: Hebrajczyków                   ",
                "20 Jk: Jakuba                          ",
                "21 1P: 1 Piotra                        ",
                "22 2P: 2 Piotra                        ",
                "23 1J: 1 Jana                          ",
                "24 2J: 2 Jana                          ",
                "25 3J: 3 Jana                          ",
                "26 Jud: Judy                           ",
                "27 Obj: Objawienie                     "
            };

            foreach (var book in books)
            {
                BookDomainObject bookDomainObject = new BookDomainObject();
                var values = book.Trim().Split(' ').ToList();
                bookDomainObject.BookName = values[1].Replace(':', ' ').Trim();
                bookDomainObject.BookGlobalNumber = Int32.Parse(values[0]);
                bookDomainObject.Guid = Guid.NewGuid();
                values.RemoveAt(0);
                values.RemoveAt(0);
                bookDomainObject.BookFullName = string.Join(" ", values);
                bookDomainObject.SubbookNumber = 0;

                bookDomainObjects.Add(bookDomainObject);
            }
            return bookDomainObjects;
        }


        private static void UpdateBooks()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var books = dbContext.BooksExtended.ToList();
                foreach (var book in books)
                {
                    book.Passages = dbContext.Passages.Where(x => x.BookGuid == book.Guid).ToList();
                    book.BeforeSave();
                }
                dbContext.SaveChanges();
            }

        }

        private static void SetNextPreviousBooks()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                var books = dbContext.BooksExtended.ToList();

                foreach (var bookExtendedDomainObject in books)
                {
                    var previousBook = books
                        .FirstOrDefault(x => x.BookFullName == bookExtendedDomainObject.BookFullName &&
                                             x.SubbookNumber == bookExtendedDomainObject.SubbookNumber - 1);

                    if (previousBook != null)
                        bookExtendedDomainObject.PreviousBookGuid = previousBook.Guid;

                    if (previousBook == null)
                        bookExtendedDomainObject.PreviousBookGuid = Guid.Empty;

                    var nextBook = books
                        .FirstOrDefault(x => x.BookFullName == bookExtendedDomainObject.BookFullName &&
                                             x.SubbookNumber == bookExtendedDomainObject.SubbookNumber + 1);

                    if (nextBook != null)
                        bookExtendedDomainObject.NextBookGuid = nextBook.Guid;
                }
                dbContext.SaveChanges();
            }
        }

        private static void ScrapeTitleAndMeantitle()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var passageList = dbContext.Passages.ToList();
                foreach (var passageDomainObject in passageList)
                {
                    //passageDomainObject.Title2 = passageDomainObject.PassageText.FindInternalOf("<br><div class=tytul2>", "</div>");
                    passageDomainObject.PassageText =
                        passageDomainObject.PassageText.RemoveWithInternal("<br><div class=tytul2>", "</div>");

                    //passageDomainObject.Title1 = passageDomainObject.PassageText.FindInternalOf("<br><div class=tytul1>", "</div>");
                    passageDomainObject.PassageText =
                        passageDomainObject.PassageText.RemoveWithInternal("<br><div class=tytul1>", "</div>");

                    //passageDomainObject.Meantitle1 = passageDomainObject.PassageText.FindInternalOf("<br><div class=miedzytytul1>", "</div>");
                    passageDomainObject.PassageText =
                        passageDomainObject.PassageText.RemoveWithInternal("<br><div class=miedzytytul1>", "</div>");

                    //passageDomainObject.Meantitle2 = passageDomainObject.PassageText.FindInternalOf("<br><div class=miedzytytul2>", "</div>");
                    passageDomainObject.PassageText =
                        passageDomainObject.PassageText.RemoveWithInternal("<br><div class=miedzytytul2>", "</div>");
                }
                dbContext.SaveChanges();
            }
        }

        private static void SetSubbookNumber()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //var passages = db.Passages.ToList();
                ////list.ForEach(x => x.PassageText = x.PassageText.Replace("£","Ł"));
                ////list.ForEach(x => x.Book = x.Book.Replace("£","Ł"));
                ////db.SaveChanges();

                var bookDomainObjects = db.Books.ToList();
                //list2.ForEach(x => x.BookName = x.BookName.Replace("£", "Ł"));
                //list2.ForEach(x => x.BookFullName = x.BookFullName.Replace("£", "Ł"));
                //db.SaveChanges();

                foreach (var book in bookDomainObjects)
                {
                    int ostatnia = book.BookName[book.BookName.Length - 1] - 48;
                    int przedostatnia = book.BookName[book.BookName.Length - 2] - 48;

                    book.SubbookNumber = przedostatnia < 10 && przedostatnia >= 0 ? przedostatnia * 10 + ostatnia : ostatnia;
                    if (book.BookName.Length - 3 >= 0)
                    {
                        int przedostatnia2 = book.BookName[book.BookName.Length - 3] - 48;
                        if (przedostatnia2 >= 0 && przedostatnia2 < 10)
                            book.SubbookNumber += przedostatnia2 * 100;
                    }
                }

                var bookDomainObjects2 = db.BooksExtended.ToList();
                //list2.ForEach(x => x.BookName = x.BookName.Replace("£", "Ł"));
                //list2.ForEach(x => x.BookFullName = x.BookFullName.Replace("£", "Ł"));
                //db.SaveChanges();

                foreach (var book in bookDomainObjects2)
                {
                    int ostatnia = book.BookName[book.BookName.Length - 1] - 48;
                    int przedostatnia = book.BookName[book.BookName.Length - 2] - 48;

                    book.SubbookNumber = przedostatnia < 10 && przedostatnia >= 0 ? przedostatnia * 10 + ostatnia : ostatnia;
                    if (book.BookName.Length - 3 >= 0)
                    {
                        int przedostatnia2 = book.BookName[book.BookName.Length - 3] - 48;
                        if (przedostatnia2 >= 0 && przedostatnia2 < 10)
                            book.SubbookNumber += przedostatnia2 * 100;
                    }
                    //book.OnRead();
                    //book.
                    //book.BeforeSave();
                }

                //foreach (var bookDomainObject in bookDomainObjects)
                //{
                //    var bookExtended = new BookExtendedDomainObject();
                //    bookExtended.BookName = bookDomainObject.BookName;
                //    bookExtended.BookFullName = bookDomainObject.BookFullName;
                //    bookExtended.Guid = bookDomainObject.Guid;
                //    bookExtended.Comments = new List<CommentDomainObject>();
                //    bookExtended.Passages = passages.Where(x => x.BookGuid == bookDomainObject.Guid).ToList();

                //    bookExtended.BeforeSave();
                //    db.BooksExtended.Add(bookExtended);
                //}
                db.SaveChanges();

                //WebScrape();
            }
        }

        private static void ReplaceLetters()
        {
            using (var dbCOntext = new ApplicationDbContext())
            {
                dbCOntext.BooksExtended.ToList().ForEach(x =>
                {
                    x.OnRead();
                    x.Passages.ForEach(y => y.PassageText = y.PassageText.Replace('¡', 'Ś'));
                    x.BeforeSave();
                });
                dbCOntext.Passages.ToList().ForEach(x => x.PassageText = x.PassageText.Replace('¡', 'Ś'));
                dbCOntext.SaveChanges();
            }
        }

        private static void WebScrape()
        {
            try
            {
                for (int j = 1; j <= 1110; j++)
                {
                    string urlAddress = "http://biblia.deon.pl/rozdzial.php?id=" + j;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;

                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream,
                                    Encoding.GetEncoding(response.CharacterSet));
                            }


                            string data = readStream.ReadToEnd();
                            //Encoding iso = Encoding.GetEncoding(28592);
                            //Encoding unicode = Encoding.UTF8;
                            //byte[] isoBytes = iso.GetBytes(data);
                            //byte[] utfBytes = Encoding.Convert(iso, unicode, isoBytes);

                            ////CultureInfo.CreateSpecificCulture("pl-PL");// = new CultureInfo("pl-PL");
                            data = data.Replace('¶', 'ś');
                            data = data.Replace('³', 'ł');
                            data = data.Replace('æ', 'ć');
                            data = data.Replace('±', 'ą');
                            data = data.Replace('|', 'Ś');
                            data = data.Replace('¦', 'ś');
                            data = data.Replace('ê', 'ę');
                            data = data.Replace('¿', 'ż');
                            data = data.Replace('¯', 'Ż');
                            data = data.Replace('ñ', 'ń');
                            data = data.Replace('¼', 'ź');
                            data = data.Replace("£", "Ł");
                            data = data.Replace("&laquo;", "\"");
                            data = data.Replace("&raquo;", "\"");
                            //data = iso.GetString(utfBytes);
                            BookDomainObject book = new BookDomainObject();
                            book.BookName =
                                data.FindInternalOf(
                                    "<title>Biblia Tysiąclecia - Pismo święte Starego i Nowego Testamentu - ",
                                    "</title>");
                            book.BookFullName = data.FindInternalOf("<div class=\"book-label\">", "</div>");
                            book.Guid = Guid.NewGuid();

                            const string contentStartString = "<div class=\"the-content\">";

                            data = data.FindInternalOf(contentStartString, "<!-- END the-content -->");

                            var bookFromDb = db.Books.Where(x => x.BookName == book.BookName).FirstOrDefault();

                            if (bookFromDb != null)
                            {
                                //int ostatnia = book.BookName[book.BookName.Length - 1] - 48;
                                //int przedostatnia = book.BookName[book.BookName.Length - 2] - 48;

                                //bookFromDb.SubbookNumber = przedostatnia < 48 && przedostatnia < 58 ? przedostatnia * 10 + ostatnia: ostatnia ;
                                //if (book.BookName.Length - 3 >= 0)
                                //{
                                //    int przedostatnia2 = book.BookName[book.BookName.Length - 3] - 48;
                                //    if (przedostatnia2 > 48 && przedostatnia2 < 57)
                                //        bookFromDb.SubbookNumber += przedostatnia2 * 100;
                                //}
                                bookFromDb.BookGlobalNumber = j;
                                var bookExtended = db.BooksExtended.Where(x => x.Guid == bookFromDb.Guid).FirstOrDefault();
                                bookExtended.BookGlobalNumber = bookFromDb.BookGlobalNumber;
                                bookExtended.SubbookNumber = bookFromDb.SubbookNumber;
                            }
                            //bool multiplied = false;
                            //List<PassageDomainObject> list = new List<PassageDomainObject>();
                            //for (int i = 1; i < 100; i++)
                            //{
                            //    string passageText;
                            //    int passageNumber;
                            //    int secondNumber;
                            //    if (data.Contains(i + "&nbsp;") || data.Contains(i + "a&nbsp;") ||
                            //        data.Contains(i + "b&nbsp;"))
                            //    {
                            //        data = ExtractWerset(data, out passageText, out passageNumber, out secondNumber);
                            //        if (secondNumber > 0 && multiplied == false)
                            //        {
                            //            multiplied = true;
                            //            list.ForEach(x => x.PassageNumber *= 10);
                            //        }

                            //        list.Add(new PassageDomainObject(passageText, book.BookName,
                            //            passageNumber + secondNumber,
                            //            book.Guid));
                            //    }
                            //}
                            try
                            {
                                //db.Passages.AddRange(list);
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                            }
                            response.Close();
                            readStream.Close();
                        }
                        Console.WriteLine(j);
                    }
                    Thread.Sleep(300);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        private static void SetGlobalNumber()
        {
            try
            {
                for (int j = 1; j <= 1110; j++)
                {
                    string urlAddress = "http://biblia.deon.pl/rozdzial.php?id=" + j;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream receiveStream = response.GetResponseStream();
                            StreamReader readStream = null;

                            if (response.CharacterSet == null)
                            {
                                readStream = new StreamReader(receiveStream);
                            }
                            else
                            {
                                readStream = new StreamReader(receiveStream,
                                    Encoding.GetEncoding(response.CharacterSet));
                            }


                            string data = readStream.ReadToEnd();

                            data = data.Replace('¶', 'ś');
                            data = data.Replace('³', 'ł');
                            data = data.Replace('æ', 'ć');
                            data = data.Replace('±', 'ą');
                            data = data.Replace('|', 'Ś');
                            data = data.Replace('¦', 'ś');
                            data = data.Replace('ê', 'ę');
                            data = data.Replace('¿', 'ż');
                            data = data.Replace('¯', 'Ż');
                            data = data.Replace('ñ', 'ń');
                            data = data.Replace('¼', 'ź');
                            data = data.Replace("£", "Ł");
                            data = data.Replace("&laquo;", "\"");
                            data = data.Replace("&raquo;", "\"");
                            //data = iso.GetString(utfBytes);
                            BookDomainObject book = new BookDomainObject();
                            book.BookName =
                                data.FindInternalOf(
                                    "<title>Biblia Tysiąclecia - Pismo święte Starego i Nowego Testamentu - ",
                                    "</title>");
                            book.BookFullName = data.FindInternalOf("<div class=\"book-label\">", "</div>");
                            book.Guid = Guid.NewGuid();

                            const string contentStartString = "<div class=\"the-content\">";

                            data = data.FindInternalOf(contentStartString, "<!-- END the-content -->");

                            var bookFromDb = db.Books.Where(x => x.BookName == book.BookName).FirstOrDefault();

                            if (bookFromDb != null)
                            {

                                bookFromDb.BookGlobalNumber = j;
                                var bookExtended = db.BooksExtended.Where(x => x.Guid == bookFromDb.Guid).FirstOrDefault();
                                bookExtended.BookGlobalNumber = bookFromDb.BookGlobalNumber;
                                bookExtended.SubbookNumber = bookFromDb.SubbookNumber;
                            }

                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                            }
                            response.Close();
                            readStream.Close();
                        }
                        Console.WriteLine(j);
                    }
                    Thread.Sleep(300);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        public static string ExtractWerset(string data, out string passage, out int passageNumber, out int secondNumber)
        {
            int indexToDestoy = data.IndexOf("<a name=\"W");
            data = data.Substring(indexToDestoy + 10);
            string passageNumberString = data.FindInternalOf("<span class=\"werset\">", "&nbsp;");

            if (passageNumberString.Contains("b"))
                secondNumber = 2;
            else if (passageNumberString.Contains("a"))
                secondNumber = 1;
            else
                secondNumber = 0;

            passageNumberString = passageNumberString.Replace("b", "");
            passageNumberString = passageNumberString.Replace("a", "");

            passageNumber = int.Parse(passageNumberString);

            if (secondNumber > 0)
                passageNumber *= 10;

            passage = data.FindInternalOf("</span>", "<a name=\"W");

            while (passage.Contains("<sup>") && passage.Contains("</sup>"))
            {
                try
                {
                    string interval = passage.FindInternalOf("<sup>", "</sup>");
                    if (interval.Length == 0)
                        break;
                    else
                    {
                        passage = passage.Replace(interval, "");

                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            passage = passage.Replace("<sup>", "");
            passage = passage.Replace("</sup>", "");
            passage = passage.Replace("<br />", "");


            return data;
        }
    }

}