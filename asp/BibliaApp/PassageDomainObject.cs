using System;

namespace BibliaApp
{
  
        public class PassageDomainObject
        {

            public Guid Guid { get; set; }
            //[MaxLength(Int32.MaxValue)]
            public string PassageText { get; set; }
            //[MaxLength(50)]
            public string Book { get; set; }
            public Guid BookGuid { get; set; }
            public int PassageNumber { get; set; }

            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public string Meantitle1 { get; set; }
            public string Meantitle2 { get; set; }
            public PassageDomainObject()
            {
                
            }

            public PassageDomainObject(string passageText, string book, int passageNumber, Guid bookGuid)
            {
                Guid = Guid.GenerateComb();
                PassageText = passageText;
                Book = book;
                PassageNumber = passageNumber;
                BookGuid = bookGuid;
            }
        }
    
}