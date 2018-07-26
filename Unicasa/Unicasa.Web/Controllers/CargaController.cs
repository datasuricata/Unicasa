using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unicasa.Domain.Entities;
using Unicasa.Web.Controllers.Base;
using Unicasa.Web.Models;

namespace Unicasa.Web.Controllers
{
    public class CargaController : BaseController
    {
        // GET: Relatorio
        public ActionResult Index()
        {
            var vm = new CargaModel();

            //get list view

            return View(vm);
        }

        public ActionResult Upload()
        {
            var vm = new CargaModel();
            vm.Importacoes = new List<Importacao>();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Upload(CargaModel vm, HttpPostedFileBase attachmentcsv)
        {
            if (attachmentcsv == null)
                return View();

            vm.Importacoes = new List<Importacao>();

            StreamReader streamReader = new StreamReader(attachmentcsv.InputStream);

            var lista = new List<string>();

            //Read the contents of CSV file.
            using (var file = streamReader)
            {
                var line = string.Empty;

                while ((line = file.ReadLine()) != null)
                {
                    lista.Add(line);
                }
            }

            //Execute a loop over the rows.
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
                        CpfCnpj = row.Split('|')[23]
                    });
                }
            }

            return Redirect("Upload");
        }
    }
}