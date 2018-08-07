using System;
using System.Collections.Generic;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.Web.Models
{
    public class RelatorioModel
    {
        public List<Ticket> Tickets { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DatePeriod Periodo { get; set; }
        public TicketState TicketState { get; set; }

        public RelatorioModel()
        {
            Tickets = new List<Ticket>();
        }
    }
}