using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapMetrica : EntityTypeConfiguration<Metricas>
    {
        public MapMetrica()
        {
            ToTable("Metricas");

            Property(p => p.Id).IsRequired();
        }
    }
}