using System.Data.Entity;

namespace Unicasa.API.Persistence
{
    public class UnicasaContext : DbContext
    {
        public UnicasaContext() : base("DefaultConnection")
        {

        }

        //public DbSet<EntidadeX> Upload { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(100));

            modelBuilder.Configurations.AddFromAssembly(typeof(UnicasaContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }


}