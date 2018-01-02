using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliaApp
{

    [Table("Book")]
    internal class BookDomainObject
    {
        public Guid Guid { get; set; }
        public string BookName { get; set; }
        public string BookFullName { get; set; }
        public int SubbookNumber { get; set; }
        public int BookGlobalNumber { get; set; }

    }

}