using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class ImportacaoRequest
    {
        public IEnumerable<Importacao> Importacoes { get; set; }
        public Cargas Carga { get; set; }
    }
}
