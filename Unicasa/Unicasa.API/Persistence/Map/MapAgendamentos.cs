using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapAgendamentos : EntityTypeConfiguration<Agendamentos>
    {
        public MapAgendamentos()
        {
            ToTable("Agendamentos");

            Property(p => p.Id).IsRequired();
        }
    }
}