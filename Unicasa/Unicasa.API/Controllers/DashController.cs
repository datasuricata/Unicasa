using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Transactions;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Helper;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/info")]
    public class DashController : ControllerBase
    {
        private readonly RepositoryTickets repositoryTicket;
        private readonly RepositoryUsuario repositoryUsuario;
        private readonly UnicasaContext context;

        public DashController()
        {
            context = new UnicasaContext();
            repositoryUsuario = new RepositoryUsuario(context);
            repositoryTicket = new RepositoryTickets(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("listar")]
        [HttpGet]
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var dash = new DashResponse();

                var request = repositoryTicket.Listar();

                dash.TicketsPendentes = request.Where(x => x.TicketState == TicketState.Agendado).Count();
                dash.TotalEntrgue= request.Where(x => x.TicketState == TicketState.Entregue).Count();
                dash.TotalUsuarios = repositoryUsuario.Listar().Count();

                return Request.CreateResponse(HttpStatusCode.OK, dash);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}