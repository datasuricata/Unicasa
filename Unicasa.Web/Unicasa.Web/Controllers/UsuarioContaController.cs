using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unicasa.Web.Controllers.Base;

namespace Unicasa.Web.Controllers
{
    public class UsuarioContaController : BaseController
    {
        // GET: UserAccount
        public ActionResult Index()
        {
            return View();
        }
    }
}