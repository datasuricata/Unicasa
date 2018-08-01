using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class AgendaResponse
    {
        public Agenda Agenda { get; set; }
        public List<Agenda> Agendamentos { get; set; }

        public AgendaResponse()
        {
            Agenda = new Agenda();
            Agendamentos = new List<Agenda>();
        }
    }
}
