using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class ImportacaoResponse
    {
        public ImportacaoResponse(List<Cargas> cargas)
        {
            Cargas = cargas;
        }

        public List<Cargas> Cargas { get; set; }
    }
}
