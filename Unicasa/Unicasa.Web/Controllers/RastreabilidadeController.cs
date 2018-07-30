using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Entities;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    public class RastreabilidadeController : BaseController
    {
        // GET: Rastreabilidade
        public async Task<ActionResult> Index()
        {
            var vm = new TicketsModel();

            var request = await Get<List<Ticket>>(_Gerenciador.Listar);
            if (request != null)
                vm.Tickets = request;

            return View(vm);
        }
    }
}