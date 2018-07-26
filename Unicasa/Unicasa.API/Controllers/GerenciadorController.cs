
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Transactions;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/gerenciador")]
    public class GerenciadorController : ControllerBase
    {
        private readonly RepositoryTickets repository;
        private readonly UnicasaContext context;

        public GerenciadorController()
        {
            context = new UnicasaContext();
            repository = new RepositoryTickets(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("adicionar/unitario")]
        [HttpPost]
        public async Task<HttpResponseMessage> Adicionar(Ticket request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente (Model)");
                    return null;
                }

                var response = repository.Adicionar(request);

                if (response == null)
                {
                    Notification.Add("O Ticket não foi criado tente novamente (Entity)");
                    return null;
                }

                Notification.Add("Ticket Criado");

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("editar")]
        [HttpPut]
        public async Task<HttpResponseMessage> Editar(Ticket request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repository.Editar(request);

                if (response == null)
                {
                    Notification.Add("O Ticket não foi salvo no banco, tente novamente");
                    return null;
                }

                Notification.Add("Ticket Alterado");

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("listar")]
        [HttpGet]
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var response = repository.Listar();
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}

