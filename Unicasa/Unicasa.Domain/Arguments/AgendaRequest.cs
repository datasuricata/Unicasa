using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class AgendaRequest
    {
        public Agenda Agenda { get; set; }
        public List<Agenda> Agendamentos { get; set; }
        public IEnumerable<Agenda> Importacao { get; set; }

        public AgendaRequest()
        {
            Agenda = new Agenda();
            Agendamentos = new List<Agenda>();
        }
    }
}
