using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Arguments.Base;
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
            var login = Session["user_email"].ToString();
            var perfil = Session["PerfilEnum"].ToString();
            var request = await Post<FiltroResponse>(_Gerenciador.Filtrar, new FiltroRequest());

            var filtro = new List<Ticket>();

            if (perfil == "Administrador" || perfil == "Gerente")
                filtro = request.Tickets;
            else
                filtro = request.Tickets.Where(x => x.Operador == login || x.Operador == "").Select(s => s).ToList();

            if (request != null)
            {
                vm.Tickets = filtro;
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

        public async Task<ActionResult> DetalhesChave(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    Notifications.Add("Identificador esta vazio");

                var vm = new TicketsModel();

                var request = await GetById<GerenciadorResponse>(_Gerenciador.OberPorChave, id);

                if (request == null)
                    Notifications.Add("Não localizado");

                vm.Tickets = request.Tickets;

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

                var request = await GetById<GerenciadorResponse>(_Gerenciador.OberPorChave, id);

                if (request == null)
                {
                    SetError("Não localizado.");
                    return Redirect("Index");
                }
                var vm = new TicketsModel()
                {
                    Tickets = request.Tickets,
                    Chave = request.Tickets.FirstOrDefault().Chave,
                    Agendamento = request.Tickets.FirstOrDefault().DataAgendamento,
                    Entrega = request.Tickets.FirstOrDefault().DataEntrega,
                    Coleta = request.Tickets.FirstOrDefault().DataColeta,
                    TicketState = request.Tickets.FirstOrDefault().TicketState.ToString()
                };

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

                var command = new GerenciadorRequest()
                {
                    Agendamento = vm.Agendamento,
                    Chave = vm.Chave,
                    Coleta = vm.Coleta,
                    Entrega = vm.Entrega,
                    Ticket = vm.Ticket,
                    Tickets = vm.Tickets,
                };

                var response = await Put<BaseResponse>(_Gerenciador.Editar, command);

                if (response != null)
                {
                    if (response.Message != null)
                        SetSuccess(response.Message);
                    else
                        SetError("Não atualziados");
                }

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