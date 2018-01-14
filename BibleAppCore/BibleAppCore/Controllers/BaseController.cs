using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BibleAppCore.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Guid? GetUserGuid()
        {
            Guid value;
            return Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "guid")?.Value, out value) ? (Guid?)value : null;
        }
    }
}
