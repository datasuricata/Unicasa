using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Web.Models
{
    public class TicketsModel
    {
        public TicketsModel()
        {
            Tickets = new List<Ticket>();
        }

        public List<Ticket> Tickets { get; set; }
    }
}