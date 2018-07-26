using System.Data.Entity.ModelConfiguration;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence.Map
{
    public class MapTicket : EntityTypeConfiguration<Ticket>
    {
        public MapTicket()
        {
            ToTable("Tickets");

            Property(p => p.Id).IsRequired();
        }
    }
}