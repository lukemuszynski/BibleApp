using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.ViewModel
{
    public class BookExtended
    {
        public Guid Guid { get; set; }
        public string BookName { get; set; }
        public string BookFullName { get; set; }
        public string PassagesJson { get; set; }
        public int SubbookNumber { get; set; }
        public int BookGlobalNumber { get; set; }
        public Guid NextBookGuid { get; set; }
        public Guid PreviousBookGuid { get; set; }
        public List<Passage> Passages { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
