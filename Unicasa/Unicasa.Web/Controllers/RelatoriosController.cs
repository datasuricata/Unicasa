using System.Web.Mvc;
using Unicasa.Web.Filters;

namespace Unicasa.Web.Controllers
{
    public class RelatoriosController : Controller
    {
        [AutorizeUser]
        public ActionResult Index()
        {
            return View();
        }
    }
}