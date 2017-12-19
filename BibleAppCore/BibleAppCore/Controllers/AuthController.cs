using BibleAppApi.Models;
using BibleAppCore.DataLayer.Repository;
using BibleAppCore.Utilities.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleAppCore.Controllers
{
    [Route("api/Auth")]
    public class AuthController : Controller
    {

        private IRepository Repository { get; set; }
        public IEncyptionProvider EncyptionProvider { get; set; }

        public AuthController(IRepository repository, IEncyptionProvider encyptionProvider)
        {
            Repository = repository;
            EncyptionProvider = encyptionProvider;
        }

        public async Task<BearerToken> Login(Credentials credentials)
        {
            credentials = EncyptionProvider.HashPassword(credentials);
            BearerToken bearerToken = BearerToken.CreateBearerToken(credentials);

            return bearerToken;
        }

    }
}
