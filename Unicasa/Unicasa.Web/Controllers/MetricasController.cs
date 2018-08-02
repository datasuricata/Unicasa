using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Entities;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Filters;

namespace Unicasa.Web.Controllers
{
    [AutorizeUser]
    public class MetricasController : BaseController
    {
        // GET: Metricas
        public async Task<ActionResult> Index()
        {
            Metricas model = new Metricas();

            model = await Get<Metricas>(_Metricas.Listar);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Metricas model)
        {
            var command = model;

            var response = await Post<Metricas>(_Metricas.Editar, command);

            if (response == null)
                SetError("Metrica não alterada, tente novamente.");

            return RedirectToAction("Index");
        }
    }
}