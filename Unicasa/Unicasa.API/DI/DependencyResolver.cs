using System.Data.Entity;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.API.Transactions;
using Unicasa.Domain.Interfaces.Repositories;
using Unicasa.Domain.Interfaces.Repositories.Base;
using Unity;
using Unity.Lifetime;

namespace Unicasa.API.DI
{
    public static class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {
            container.RegisterType<DbContext, UnicasaContext>(new HierarchicalLifetimeManager());
            //UnitOfWork
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            //Repository
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            container.RegisterType<IImportacaoRepository, RepositoryImportacao>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioRepository, RepositoryUsuario>(new HierarchicalLifetimeManager());
            container.RegisterType<IMetricasRepository, RepositoryMetricas>(new HierarchicalLifetimeManager());
            container.RegisterType<ITicketRepository, RepositoryTickets>(new HierarchicalLifetimeManager());
            container.RegisterType<IFeriadosRepository, RepositoriyFeriado>(new HierarchicalLifetimeManager());
            container.RegisterType<ICargasRepository, RepositoryCargas>(new HierarchicalLifetimeManager());
        }
    }
}