using System.Data.Entity;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Persistence
{
    public class UnicasaContext : DbContext
    {
        public UnicasaContext() : base("DefaultConnection")
        {

        }

        public DbSet<Importacao> Importacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Metricas> Metricas { get; set; }
        public DbSet<Cargas> Cargas { get; set; }
        public DbSet<Feriados> Feriados { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(100));

            modelBuilder.Configurations.AddFromAssembly(typeof(UnicasaContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}