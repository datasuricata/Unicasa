using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class GerenciadorRequest
    {
        public GerenciadorRequest()
        {
            Ticket = new Ticket();
            TicketIds = new List<string>();
            Tickets = new List<Ticket>();
        }

        public Ticket Ticket { get; set; }
        public List<string> TicketIds { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
