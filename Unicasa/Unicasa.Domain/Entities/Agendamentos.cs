using System;
using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Agendamentos : BaseEntity
    {
        public DateTime? DataAgendamento { get; set;}
        public int Agendados { get; set; }

        public Agendamentos()
        {

        }
    }
}
