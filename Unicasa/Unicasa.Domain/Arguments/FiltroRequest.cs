using System;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Arguments
{
    public class FiltroRequest
    {
        public TicketState TicketState { get; set; }
        public string Chave { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime DataEntrega { get; set; }
    }
}
