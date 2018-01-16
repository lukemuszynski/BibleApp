using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BibleAppCore.Controllers
{
    public abstract class BaseController : Controller
    {
        private Guid? _userGuid;
        protected Guid? UserGuid
        {
            get
            {
                if (_userGuid == null)
                {
                    Guid value;
                    if (Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "guid")?.Value, out value))
                        _userGuid = value;
                    else
                        _userGuid = Guid.Empty;
                }
                return _userGuid != Guid.Empty ? _userGuid : null;
            }
        }
    }
}
