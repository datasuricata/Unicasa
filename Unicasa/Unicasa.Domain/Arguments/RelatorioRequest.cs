using System;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Arguments
{
    public class RelatorioRequest
    {
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DatePeriod Periodo { get; set; }
        public TicketState TicketState { get; set; }
    }
}
