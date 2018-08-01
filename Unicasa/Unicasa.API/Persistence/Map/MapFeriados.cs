using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapFeriados : EntityTypeConfiguration<Agenda>
    {
        public MapFeriados()
        {
            ToTable("Feriados");

            Property(p => p.Id).IsRequired();
        }
    }
}