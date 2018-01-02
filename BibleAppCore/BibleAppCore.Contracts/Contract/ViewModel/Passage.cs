using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.ViewModel
{
    public class Passage
    {
        public Guid Guid { get; set; }
        public string PassageText { get; set; }
        public string Book { get; set; }
        public Guid BookGuid { get; set; }
        public int PassageNumber { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Meantitle1 { get; set; }
        public string Meantitle2 { get; set; }
    }
}
