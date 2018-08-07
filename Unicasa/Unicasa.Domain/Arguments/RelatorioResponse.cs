using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class RelatorioResponse
    {
        public RelatorioResponse()
        {
            Tickets = new List<Ticket>();
        }

        public List<Ticket> Tickets { get; set; }
    }
}
