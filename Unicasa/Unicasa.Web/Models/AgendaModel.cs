using System.Collections.Generic;
using Unicasa.Domain.Entities;

namespace Unicasa.Web.Models
{
    public class AgendaModel
    {
        public AgendaModel()
        {
            Agendamentos = new List<Agenda>();
            Agenda = new Agenda();
        }

        public List<Agenda> Agendamentos { get; set; }
        public Agenda Agenda { get; set; }
    }
}