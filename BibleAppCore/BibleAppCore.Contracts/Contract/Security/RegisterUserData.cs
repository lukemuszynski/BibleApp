using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.Security
{
    public class RegisterUserData
    {
        public string EmailAddress { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
