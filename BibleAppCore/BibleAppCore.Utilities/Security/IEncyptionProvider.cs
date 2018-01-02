using BibleAppApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using BibleAppCore.Contracts.Contract.Security;

namespace BibleAppCore.Utilities.Security
{
    public interface IEncyptionProvider
    {
        string HashPassword(string credentials);
        BearerToken CreateBearerToken(BearerToken bearerToken);
    }
}
