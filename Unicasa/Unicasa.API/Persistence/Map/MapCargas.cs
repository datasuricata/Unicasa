using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapCargas : EntityTypeConfiguration<Cargas>
    {
        public MapCargas()
        {
            ToTable("Cargas");

            Property(p => p.Id).IsRequired();
        }
    }
}