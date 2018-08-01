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
using Unicasa.Domain.Entities;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/metricas")]
    public class MetricasController : ControllerBase
    {
        private readonly RepositoryMetricas repositoryMetricas;
        private readonly UnicasaContext context;

        public MetricasController()
        {
            context = new UnicasaContext();
            repositoryMetricas = new RepositoryMetricas(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("listar")]
        [HttpGet]
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var response = repositoryMetricas.Listar().FirstOrDefault();

                if (response == null)
                {
                    Notification.Add("Erro ao listar dados no banco, tente novamente");
                    return null;
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("editar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Editar(Metricas request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repositoryMetricas.Editar(request);

                if (response == null)
                {
                    Notification.Add("Erro ao salvar dados no banco, tente novamente");
                    return null;
                }
                return await ResponseAsync(response);

            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("adicionar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Adicionar(Metricas request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repositoryMetricas.Adicionar(request);

                if (response == null)
                {
                    Notification.Add("A metrica não foi salve no banco, tente novamante");
                    return null;
                }

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("excluir")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Exluir(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var usuario = repositoryMetricas.ObterPorId(id);
                repositoryMetricas.Remover(usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

    }
}