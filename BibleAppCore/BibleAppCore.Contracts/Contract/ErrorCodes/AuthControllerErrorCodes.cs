using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Contracts.Contract.ErrorCodes
{
    public struct AuthControllerErrorCodes
    {
        public const string LoginUsedError = "LOGIN_IS_USED";
        public const string EmailUsedError = "EMAIL_IS_USED";
        public const string LoginAndEmailUsedError = "EMAIL_AND_LOGIN_IS_USED";
    }
}
