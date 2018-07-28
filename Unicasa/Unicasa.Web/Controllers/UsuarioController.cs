using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: User
        public async Task<ActionResult> Index()
        {
            var response = await Get<List<Usuario>>(_Usuario.Listar);

            var vm = new UsuarioModel();
            vm.Usuarios = response;
            vm.Perfis = Components.GetDrowdown<UserRole>();

            return View(vm);
        }
    }
}