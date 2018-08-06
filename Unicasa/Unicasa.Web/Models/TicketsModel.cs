using System;
using System.Collections.Generic;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.Web.Models
{
    public class TicketsModel
    {
        public TicketsModel()
        {
            Ticket = new Ticket();
            Tickets = new List<Ticket>();
            Importacao = new Importacao();
            DropdownEnums = new List<GenericDropdown>();
        }

        public List<Ticket> Tickets { get; set; }
        public Importacao Importacao  { get; set; }
        public Ticket Ticket { get; set; }
        public List<GenericDropdown> DropdownEnums { get; set; }

        public string Chave { get; set; }
        public TicketState TicketState { get; set; }
        public DateTime? Agendamento { get; set; }
        public DateTime? Entrega { get; set; }
        public DateTime? Coleta { get; set; }
    }
}