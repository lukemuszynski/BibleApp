using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BibleAppCore.Contracts.Contract.ViewModel;

namespace BibleAppApi.Models
{
    public class Book
    {
        public Guid Guid { get; set; }
        public string BookName { get; set; }
        public string BookFullName { get; set; }
        public int SubbookNumber { get; set; }
        public int StartGlobalIndex { get; set; }
        public int BookGlobalNumber { get; set; }
        public List<Subbook> Subbooks { get; set; }
    }
}