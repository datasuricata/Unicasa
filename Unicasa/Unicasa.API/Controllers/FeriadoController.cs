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
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/feriado")]
    public class FeriadoController : ControllerBase
    {
        private readonly RepositoriyFeriado repository;
        private readonly UnicasaContext context;

        public FeriadoController()
        {
            context = new UnicasaContext();
            repository = new RepositoriyFeriado(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("adicionar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Adicionar(Feriados request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repository.Adicionar(Feriados.Registrar(request));

                if (response == null)
                {
                    Notification.Add("O feriado não foi salve no banco, tente novamante");
                    return null;
                }

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("importar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Importar(IEnumerable<Feriados> request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("O arquivo não foi carregado, por favor tente novamente (Model)");
                    return null;
                }

                var response = repository.AdicionarLista(request);

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

        [Route("obterPorId")]
        [HttpGet]
        public async Task<HttpResponseMessage> ObterPorId(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repository.ObterPorId(id);

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
                var response = repository.Listar().ToList();
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("editar")]
        [HttpPut]
        public async Task<HttpResponseMessage> Editar(Feriados request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }
                var feriado = repository.ObterPorId(request.Id);
                var response = repository.Editar(Feriados.Editar(feriado, request));

                if (response == null)
                {
                    Notification.Add("O feriado não foi salvo no banco, tente novamante");
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

                var usuario = repository.ObterPorId(id);
                repository.Remover(usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}