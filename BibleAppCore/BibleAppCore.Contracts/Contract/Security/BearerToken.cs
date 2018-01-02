using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BibleAppApi.Models
{
    public class BearerToken
    {
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }   
    }
}