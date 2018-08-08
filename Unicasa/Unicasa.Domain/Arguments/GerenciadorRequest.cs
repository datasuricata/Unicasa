using System;
using System.Collections.Generic;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

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
        public string Chave { get; set; }
        public TicketState TicketState { get; set; }
        public DateTime? Agendamento { get; set; }
        public DateTime? Coleta { get; set; }
        public DateTime? Entrega { get; set; }
        public string Observacao { get; set; }

    }
}
