using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Transactions;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ControllerBase
    {
        private readonly RepositoryTickets repositoryTickets;
        private readonly UnicasaContext context;

        public RelatoriosController()
        {
            context = new UnicasaContext();
            repositoryTickets = new RepositoryTickets(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("agendamentos")]
        [HttpPost]
        public async Task<HttpResponseMessage> Agendamentos(RelatorioRequest request)
        {
            try
            {
                var lista = Filtro(request);
                if (lista == null){Notification.Add("Tickets não encontrados."); return null;}
                var response = new RelatorioResponse(){Tickets = lista};
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("leituras")]
        [HttpPost]
        public async Task<HttpResponseMessage> Leituras(RelatorioRequest request)
        {
            try
            {
                var lista = Filtro(request);
                if (lista == null){Notification.Add("Tickets não encontrados.");return null;}
                var response = new RelatorioResponse(){Tickets = lista};
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        #region [ Privates ]

        private List<Ticket> Filtro(RelatorioRequest request)
        {
            var query = repositoryTickets.Listar().OrderBy(x => x.Chave).ToList();

            var filtrado = new List<Ticket>();

            if (request.TicketState == TicketState.Agendado)
                filtrado = query.Select(s => s).Where(e => e.TicketState == TicketState.Agendado).ToList();

            if (request.TicketState == TicketState.Selecione)
                filtrado = query.Select(s => s).Where(e => e.DataColeta != null).ToList();

            if (request.DataInicial != null && request.DataFinal != null)
                filtrado = query.Select(s => s).Where(x => x.DataAgendamento >= request.DataInicial.Value && x.DataAgendamento <= request.DataFinal.Value).ToList();

            if (request.Periodo == DatePeriod.Semanal)
                filtrado = query.Select(s => s).Where(x => x.DataAgendamento >= DateTime.Now && x.DataAgendamento <= (DateTime.Now.AddDays(-7))).ToList();

            if (request.Periodo == DatePeriod.Quinzenal)
                filtrado = query.Select(s => s).Where(x => x.DataAgendamento >= DateTime.Now && x.DataAgendamento <= (DateTime.Now.AddDays(-15))).ToList();

            if (request.Periodo == DatePeriod.Mensal)
                filtrado = query.Select(s => s).Where(x => x.DataAgendamento >= DateTime.Now && x.DataAgendamento <= (DateTime.Now.AddDays(-30))).ToList();

            return filtrado;
        }

        #endregion
    }
}