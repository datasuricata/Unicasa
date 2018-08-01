using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    public class AgendaController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var response = await Get<AgendaResponse>(_Agenda.Listar);

            var vm = new AgendaModel();
            vm.Agendamentos = response.Agendamentos;

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Create(AgendaModel vm)
        {

            var command = vm.Agenda;

            var response = await Post<BaseResponse>(_Agenda.Adicionar, command);

            if (response == null)
                SetError("Agendamento não criado, tente novamente.");

            else
                SetSuccess(response.Message);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update(string id)
        {
            var vm = new AgendaModel();

            var request = await GetById<Agenda>(_Agenda.ObterPorId, id);

            if (request == null)
                SetError("Erro ao obter usuário.");

            vm.Agenda = request;

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Update(AgendaModel vm)
        {
            var command = vm.Agenda;

            var response = await Post<BaseResponse>(_Agenda.Editar, command);

            if (response == null)
                SetError("Agenda não alterada, tente novamente.");
            else
                SetSuccess(response.Message);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var response = await GetById<HttpStatusCode>(_Agenda.Excluir, id);
            return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            var vm = new CargaModel();
            vm.Importacoes = new List<Importacao>();

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(AgendaModel vm, HttpPostedFileBase attachmentcsv)
        {
            if (attachmentcsv == null)
                return View();

            vm.Agendamentos = new List<Agenda>();

            StreamReader stream = new StreamReader(attachmentcsv.InputStream);

            var lista = new List<string>();

            using (var file = stream)
            {
                var line = string.Empty;

                while ((line = file.ReadLine()) != null)
                {
                    lista.Add(line);
                }
            }
            foreach (string row in lista)
            {
                if (!string.IsNullOrEmpty(row))
                {
                    vm.Agendamentos.Add(new Agenda
                    {
                        Ativo = true,
                        DataFeriado = Convert.ToDateTime(row.Split('|')[0]),
                        Titulo = row.Split('|')[1]
                    });
                }
            }

            var response = new AgendaRequest()
            {
                Importacao = vm.Agendamentos
            };

            var request = await Post<BaseResponse>(_Importacao.Importar, response);

            if (request == null)
                SetError("Falha na importação");

            return Redirect("Index");
        }

    }
}