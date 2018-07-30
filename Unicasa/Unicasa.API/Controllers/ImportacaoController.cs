using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Transactions;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/importacao")]
    public class ImportacaoController : ControllerBase
    {
        private readonly RepositoryImportacao repositoryImportacao;
        private readonly RepositoryCargas repositoryCargas;
        private readonly UnicasaContext context;

        public ImportacaoController()
        {
            context = new UnicasaContext();
            repositoryImportacao = new RepositoryImportacao(context);
            repositoryCargas = new RepositoryCargas(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("importar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Importar(ImportacaoRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("O arquivo não foi carregado, por favor tente novamente.");
                    return null;
                }

                var carga = new Cargas();
                var importacoes = request.Importacoes;

                carga = request.Carga;
                var domain = repositoryCargas.Adicionar(carga);

                if (domain == null)
                {
                    Notification.Add("O arquivo não pode ser carregado, por favor tente novamente.");
                    return null;
                }

                var response = repositoryImportacao.AdicionarLista(importacoes);

                if (response == null)
                {
                    Notification.Add("Erro ao salvar dados no banco, tente novamente.");
                    return null;
                }

                var message = new BaseResponse();

                return await ResponseAsync(message);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("importar/registro")]
        [HttpPost]
        public async Task<HttpResponseMessage> ImportarRegistro(Importacao request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("A importação esta vazia, por favor tente novamente");
                    return null;
                }

                var response = repositoryImportacao.Adicionar(request);

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

        [Route("listar")]
        [HttpGet]
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var response = repositoryImportacao.Listar().ToList();

                if (response == null)
                {
                    Notification.Add("Erro ao listar dados no banco, tente novamente");
                    return null;
                }

                var importacao = new ImportacaoRequest();
                importacao.Importacoes = response;

                return await ResponseAsync(importacao);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("listar/cargas")]
        public async Task<HttpResponseMessage> ListarCargas()
        {
            try
            {
                var response = repositoryCargas.Listar().ToList();

                if (response == null)
                {
                    Notification.Add("Erro ao listar dados no banco, tente novamente");
                    return null;
                }

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}