using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Web.Controllers.Base;

namespace Unicasa.Web.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var model = new DashResponse();

            var request = await Get<DashResponse>(_Dash.Listar);
            model = request;

            return View(model);
        }
    }
}