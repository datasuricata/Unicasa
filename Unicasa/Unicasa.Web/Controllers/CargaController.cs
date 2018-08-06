using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unicasa.Dashboard.Requests.Endpoints;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Entities;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Filters;
using Unicasa.Web.Helpers.Exceptions;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    [AutorizeUser]
    public class CargaController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var vm = new CargaModel();

            vm.Importacoes = new List<Importacao>();
            var request = await Get<ImportacaoResponse>(_Importacao.ListarCargas);

            if (request != null)
                vm.Cargas = request.Cargas;

            return View(vm);
        }

        public ActionResult Upload()
        {
            var vm = new CargaModel();
            vm.Importacoes = new List<Importacao>();

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(CargaModel vm, HttpPostedFileBase attachmentcsv)
        {
            try
            {
                if (attachmentcsv == null)
                    return View();

                vm.Importacoes = new List<Importacao>();

                StreamReader stream = new StreamReader(attachmentcsv.InputStream);

                var carga = new Cargas()
                {
                    NomeArquivo = attachmentcsv.FileName,
                    UsuarioImportacao = Environment.UserName.ToString(),
                    Observacao = vm.Observacoes
                };
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
                        vm.Importacoes.Add(new Importacao
                        {
                            Lote = row.Split('|')[0],
                            CodTransportadora = row.Split('|')[1],
                            Pedido = row.Split('|')[2],
                            Descricao = row.Split('|')[3],
                            NumVolume = row.Split('|')[4],
                            TotalVolume = row.Split('|')[5],
                            OrdCompra = row.Split('|')[6],
                            Carga = row.Split('|')[7],
                            RefItem = row.Split('|')[8],
                            Barra = row.Split('|')[9],
                            Situcao = row.Split('|')[10],
                            Cliente = row.Split('|')[11],
                            Endereco = row.Split('|')[12],
                            Cidade = row.Split('|')[13],
                            UF = row.Split('|')[14],
                            Quantidade = row.Split('|')[15],
                            Documento = "",
                            Peso = row.Split('|')[17],
                            Cubagem = row.Split('|')[18],
                            SubFamilia = row.Split('|')[19],
                            Fechamento = row.Split('|')[20],
                            Esteira = row.Split('|')[21],
                            Expedicao = row.Split('|')[22],
                            CpfCnpj = row.Split('|')[23],
                            Importado = false,
                            Entregue = false,
                            CargaId = carga.Id,
                        });
                    }
                }

                var importacao = new ImportacaoRequest()
                {
                    Importacoes = vm.Importacoes,
                    Carga = carga
                };

                var request = await Post<BaseResponse>(_Importacao.Importar, importacao);

                if (request == null)
                    SetError("Falha na importação");

                return RedirectToAction("Sincronizar", new { id = request.Id });
            }
            catch (ApiException ex)
            {
                SetError(ex.Message);
                return Redirect("Upload");
            }
        }

        public async Task<ActionResult> Sincronizar(string id)
        {
            if (id == null)
            {
                SetError("Id é vazio");
                return RedirectToAction("Index");
            }

            var request = await GetById<bool>(_Gerenciador.Sincronizar, id);

            if (!request)
            {
                SetError("Erro sincronizar tickets");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Rastreabilidade");
        }

        public async Task<ActionResult> Excluir(string id)
        {
            if (id == null)
            {
                SetError("Id é vazio");
                return RedirectToAction("Index");
            }

            var request = await Put<BaseResponse>(_Importacao.Excluir, id);

            if (request == null)
            {
                SetError("Erro sincronizar tickets");
                return RedirectToAction("Index");
            }

            SetSuccess(request.Message);

            return RedirectToAction("Index", "Rastreabilidade");
        }
    }
}