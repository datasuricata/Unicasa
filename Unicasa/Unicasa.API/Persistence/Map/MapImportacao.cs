using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapImportacao : EntityTypeConfiguration<Importacao>
    {
        public MapImportacao()
        {
            ToTable("Importacoes");

            Property(p => p.Id).IsRequired();
        }
    }
}