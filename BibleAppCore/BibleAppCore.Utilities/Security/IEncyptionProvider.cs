using BibleAppApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Utilities.Security
{
    public interface  IEncyptionProvider
    {
        Credentials HashPassword(Credentials credentials);
    }
}
