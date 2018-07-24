using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapUsuario : EntityTypeConfiguration<Usuario>
    {
        public MapUsuario()
        {
            ToTable("Usuarios");

            Property(p => p.Id).IsRequired();
        }
    }
}