using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Unicasa.Domain.Interfaces.Repositories;
using Unicasa.Domain.Interfaces.Services;
using Unicasa.Domain.Interfaces.Services.Base;

namespace Unicasa.Domain.Services
{
    public class ImportacaoService : IBaseService, IImportacaoService
    {
        List<string> Notification = new List<string>();

        private readonly IImportacaoRepository importacaoRepository;

        public ImportacaoService(IImportacaoRepository importacaoRepository)
        {
            this.importacaoRepository = importacaoRepository;
        }

        public Task CarregarArquivo(HttpPostedFileBase file)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            Dispose();
        }

        public List<string> Notifications()
        {
            return Notification;
        }
    }
}
