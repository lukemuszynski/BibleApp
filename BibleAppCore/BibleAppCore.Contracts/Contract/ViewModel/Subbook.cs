using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.ViewModel
{
    public class Subbook
    {
        public Guid Guid { get; set; }
        public string BookName { get; set; }
        public string BookFullName { get; set; }
        public int SubbookNumber { get; set; }
        public int BookGlobalNumber { get; set; }
    }
}
