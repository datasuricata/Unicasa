using System.Web.Mvc;
using Unicasa.Web.Controllers.Base;

namespace Unicasa.Web.Controllers
{
    public class CargaController : BaseController
    {
        // GET: Relatorio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Upload()
        //{
        //    return View();
        //}
    }
}