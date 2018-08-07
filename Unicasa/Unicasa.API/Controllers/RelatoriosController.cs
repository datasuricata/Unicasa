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
                return Request.CreateResponse(HttpStatusCode.OK, response);
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
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        #region [ Privates ]

        private List<Ticket> Filtro(RelatorioRequest request)
        {
            var query = repositoryTickets.Listar();

            if (request.TicketState == TicketState.Agendado)
                query.Where(e => e.TicketState == request.TicketState);

            if (request.TicketState == TicketState.Selecione)
                query.Where(e => e.DataColeta != null);

            if (request.DataInicial != null && request.DataFinal != null)
                query.Where(x => x.DataAgendamento >= request.DataInicial && x.DataAgendamento <= request.DataFinal);

            if (request.Periodo == DatePeriod.Semanal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-7)));

            if (request.Periodo == DatePeriod.Quinzenal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-15)));

            if (request.Periodo == DatePeriod.Mensal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-30)));

            return query.ToList();
        }

        #endregion
    }
}