using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BibleAppApi.Models;
using BibliaApp;

namespace BibleAppApi.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            
        }


        [HttpPost]
        public async Task<BearerToken> Login(Credentials credentials)
        {
            return new BearerToken();
        }

        public async Task<BearerToken> RegisterUser(UserDomainObject userDomainObject)
        {

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                //userDomainObject.Password = 
            }
            throw new NotImplementedException();
        }

    }
}