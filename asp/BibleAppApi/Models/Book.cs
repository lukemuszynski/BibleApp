using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BibliaApp;

namespace BibleAppApi.Models
{
    public class Book
    {
        public string BookFullName { get; set; }
        public int StartGlobalIndex { get; set; }
        public List<BookDomainObject> Subbooks { get; set; }
    }
}