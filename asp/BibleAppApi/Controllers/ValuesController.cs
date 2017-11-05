using BibliaApp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BibleAppApi.Controllers
{
    public class ValuesController : ApiController
    {

        // GET api/values
        public async Task<BookExtendedDomainObject> Get()
        {
            BookExtendedDomainObject bokkBookExtendedDomainObject;

            using (var dbContext = new ApplicationDbContext())
            {
                bokkBookExtendedDomainObject =  await dbContext.BooksExtended.FirstOrDefaultAsync();
            }
            bokkBookExtendedDomainObject.OnRead();
            return bokkBookExtendedDomainObject;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
