using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    public class RastreabilidadeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var vm = new TicketsModel();

            var request = await Post<FiltroResponse>(_Gerenciador.Filtrar, new FiltroRequest());

            if (request != null)
                vm.Tickets = request.Tickets;

            return View(vm);
        }
    }
}