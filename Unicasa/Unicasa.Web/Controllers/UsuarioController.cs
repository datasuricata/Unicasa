using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unicasa.Web.Controllers.Base;

namespace Unicasa.Web.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
    }
}