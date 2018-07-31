using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class TicketRequest
    {
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
