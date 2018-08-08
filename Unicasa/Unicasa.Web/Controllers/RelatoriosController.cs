using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Helper;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Filters;
using Unicasa.Web.Models;
using Unicasa.Web.Requests.Endpoints;

namespace Unicasa.Web.Controllers
{
    [AutorizeUser]
    public class RelatoriosController : BaseController
    {
        #region [ GET ]

        public ActionResult Agendamentos()
        {
            var viewModel = new RelatorioModel();
            return View(viewModel);
        }

        public ActionResult Leituras()
        {
            var viewModel = new RelatorioModel();
            return View(viewModel);
        }

        #endregion

        #region [ POST ]

        [HttpPost]
        public async Task<ActionResult> Agendamentos(RelatorioModel vm)
        {
            var response = new RelatorioResponse();
            var viewModel = new RelatorioModel();

            if (vm != null)
            {
                var request = new RelatorioRequest() { DataFinal = vm.DataFinal, DataInicial = vm.DataInicial, Periodo = vm.Periodo, TicketState = TicketState.Agendado };
                response = await Post<RelatorioResponse>(_Relatorios.Agendamentos, request);
                viewModel.Tickets = response.Tickets;
            }
            else
            {
                var request = new RelatorioRequest() { DataFinal = null, DataInicial = null, Periodo = DatePeriod.Selecione, TicketState = TicketState.Agendado };
                response = await Post<RelatorioResponse>(_Relatorios.Agendamentos, request);
                viewModel.Tickets = response.Tickets;
            }

            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Leituras(RelatorioModel vm)
        {
            var response = new RelatorioResponse();
            var viewModel = new RelatorioModel();

            if (vm != null)
            {
                var request = new RelatorioRequest() { DataFinal = vm.DataFinal, DataInicial = vm.DataInicial, Periodo = vm.Periodo, TicketState = TicketState.Selecione };
                response = await Post<RelatorioResponse>(_Relatorios.Agendamentos, request);
                viewModel.Tickets = response.Tickets;
            }
            else
            {
                var request = new RelatorioRequest() { DataFinal = null, DataInicial = null, Periodo = DatePeriod.Selecione, TicketState = TicketState.Selecione };
                response = await Post<RelatorioResponse>(_Relatorios.Agendamentos, request);
                viewModel.Tickets = response.Tickets;
            }

            return View(viewModel);
        }

        #endregion
    }
}