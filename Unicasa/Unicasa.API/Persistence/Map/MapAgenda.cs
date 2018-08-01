using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapAgenda : EntityTypeConfiguration<Agenda>
    {
        public MapAgenda()
        {
            ToTable("Agendamentos");

            Property(p => p.Id).IsRequired();
        }
    }
}