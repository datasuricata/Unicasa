﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Domain.Arguments;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Helpers.Exceptions;
using Unicasa.Web.Requests.Endpoints;

namespace Unicasa.Web.Controllers
{
    public class UsuarioContaController : BaseController
    {
        public ActionResult Login()
        {
            var vm = new AutenticarRequest();

            if (Session["AuthorizedUserId"] != null)
                return RedirectToAction("Index", "Home");
            else
                return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Login(AutenticarRequest vm)
        {
            //Valida login
            if (vm != null)
            {
                if (string.IsNullOrEmpty(vm.Email) || string.IsNullOrEmpty(vm.Email))
                {
                    SetError("Email ou senha invalidos.");
                    return View();
                }
            }
            else
                return View();

            //Inicia requisição
            try
            {
                var request = await Post<AutenticarResponse>(_UsuarioConta.Logar, vm);

                if (request.Id == null)
                {
                    SetError("Usuario não encontrado.");
                    return View();
                }

                #region [ Parametros de Sessão ]

                Session.Clear();
                Session.Timeout = 40;
                Session["user_loged"] = true;
                Session["user_name"] = request.Nome;
                Session["AuthorizedUserId"] = request.Id;
                Session["user_email"] = request.Email;
                Session["PerfilEnum"] = request.Perfil;
                Session["access_token"] = request.Token;

                #endregion

                return RedirectToAction("Index", "Home");
            }
            catch (ApiException api)
            {
                SetError(api.Message);
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            //todo mudar para novo metodo de validação
            if (Session["AuthorizedUserId"] == null)
            {
                Session.Abandon();
                return RedirectToAction("Login", "UsuarioConta");
            }

            Session["user_loged"] = false;
            Session["AuthorizedUserId"] = null;
            Session["FirstNameAuthorizedUser"] = null;
            Session["PerfilEnum"] = null;
            Session["access_token"] = null;
            Session.Abandon();

            return RedirectToAction("Login");
        }
    }
}