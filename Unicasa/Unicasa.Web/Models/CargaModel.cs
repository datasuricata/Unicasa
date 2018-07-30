using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Web.Models
{
    public class CargaModel
    {
        public List<Importacao> Importacoes { get; set; }
        public string Observacoes { get; set; }

        public CargaModel()
        {

        }
    }
}