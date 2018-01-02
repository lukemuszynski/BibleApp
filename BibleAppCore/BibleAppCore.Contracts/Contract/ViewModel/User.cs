using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.ViewModel
{
    public class User
    {
        public Guid Guid { get; set; }
        public string EmailAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
