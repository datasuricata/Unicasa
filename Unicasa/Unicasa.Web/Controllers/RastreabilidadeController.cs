using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Filters;
using Unicasa.Web.Helpers.Exceptions;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    [AutorizeUser]
    public class RastreabilidadeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var vm = new TicketsModel();

            var request = await Post<FiltroResponse>(_Gerenciador.Filtrar, new FiltroRequest());

            if (request != null)
            {
                vm.Tickets = request.Tickets;
                vm.DropdownEnums = Components.GetDrowdown<TicketState>();
            }


            return View(vm);
        }

        public async Task<ActionResult> Detalhes(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    Notifications.Add("Identificador esta vazio");

                var vm = new TicketsModel();
                var request = await GetById<Importacao>(_Importacao.ObterPorId, id);

                if (request == null)
                    Notifications.Add("Não localizado");

                vm.Importacao = request;

                return View(vm);
            }
            catch (ApiException)
            {
                SetErrors(Notifications);
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Update(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    SetError("Sem identificador");
                    return Redirect("Index");
                }

                var request = await GetById<GerenciadorResponse>(_Gerenciador.ObterPorId, id);

                if (request == null)
                {
                    SetError("Não localizado.");
                    return Redirect("Index");
                }
                var vm = new TicketsModel(){Ticket = request.Ticket};

                return View(vm);
            }
            catch (ApiException ex)
            {
                SetError(ex.Message);
                return Redirect("Index");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Update(TicketsModel vm)
        {
            try
            {
                if (vm == null)
                    Notifications.Add("Modelo inválido");

                var command = new GerenciadorRequest();
                command.Ticket = vm.Ticket;

                var response = await Post<GerenciadorResponse>(_Gerenciador.Editar, command);

                if (response != null)
                    SetSuccess("Ord. de serviço atualizada");

                return RedirectToAction("Index");
            }
            catch (ApiException)
            {
                SetErrors(Notifications);
                return RedirectToAction("Index");
            }
        }
    }
}