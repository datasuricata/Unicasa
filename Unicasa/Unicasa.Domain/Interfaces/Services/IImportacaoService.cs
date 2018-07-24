using System.Threading.Tasks;
using System.Web;

namespace Unicasa.Domain.Interfaces.Services
{
    public interface IImportacaoService
    {
        Task CarregarArquivo(HttpPostedFileBase file);
    }
}
