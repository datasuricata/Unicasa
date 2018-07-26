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
using Unicasa.Domain.Entities;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/importacao")]
    public class ImportacaoController : ControllerBase
    {
        private readonly RepositoryImportacao repository;
        private readonly UnicasaContext context;

        public ImportacaoController()
        {
            context = new UnicasaContext();
            repository = new RepositoryImportacao(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("importar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Importar(IEnumerable<Importacao> imports)
        {
            try
            {
                if (imports == null)
                {
                    Notification.Add("O arquivo não foi carregado, por favor tente novamente (Model)");
                    return null;
                }

                var response = repository.AdicionarLista(imports);

                if (response == null)
                {
                    Notification.Add("Erro ao salvar dados no banco, tente novamente");
                    return null;
                }

                Notification.Add("Upload Efetuado");

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }


        }

        [Route("importar/unitario")]
        [HttpPost]
        public async Task<HttpResponseMessage> ImportarUitario(Importacao import)
        {
            try
            {
                if (import == null)
                {
                    Notification.Add("A importação esta vazia, por favor tente novamente (Model)");
                    return null;
                }

                var response = repository.Adicionar(import);

                if (response == null)
                {
                    Notification.Add("Erro ao salvar dados no banco, tente novamente");
                    return null;
                }

                Notification.Add("Upload Efetuado");

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}