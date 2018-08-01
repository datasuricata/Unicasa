using System.Collections.Generic;
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
        public async Task<ActionResult> Index()
        {
            var response = await Get<List<Usuario>>(_Usuario.Listar);

            var vm = new UsuarioModel();
            vm.Usuarios = response;
            vm.Perfis = Components.GetDrowdown<UserRole>();

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioModel vm)
        {

            var command = vm.Usuario;

            var response = await Post<Usuario>(_Usuario.Adicionar, command);

            if (response == null)
                SetError("Usuário não criado, tente novamente.");

            else
                SetSuccess("Usuário: " + command.NomeCompleto + " criado com sucesso.");

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update(string id)
        {
            var vm = new UsuarioModel();
            vm.Perfis = Components.GetDrowdown<UserRole>();

            var request = await GetById<Usuario>(_Usuario.ObterPorId, id);

            if (request == null)
                SetError("Erro ao obter usuário.");

            vm.Usuario = request;

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UsuarioModel vm)
        {
            var command = vm.Usuario;

            var response = await Post<Usuario>(_Usuario.Editar, command);

            if (response == null)
                SetError("Usuário não alterado, tente novamente.");
            else
                SetSuccess("Usuário alterado.");

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var response = await GetById<string>(_Usuario.Excluir, id);
            return RedirectToAction("Index");
        }
    }
}