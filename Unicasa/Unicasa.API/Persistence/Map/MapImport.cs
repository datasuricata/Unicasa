using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapImport : EntityTypeConfiguration<Importacao>
    {
        public MapImport()
        {
            ToTable("Importacoes");

            Property(p => p.Id).IsRequired();
        }
    }
}