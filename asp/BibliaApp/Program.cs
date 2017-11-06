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

            //ScrapeTitleAndMeantitle();
            UpdateBooks();

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