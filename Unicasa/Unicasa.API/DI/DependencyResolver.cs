using System.Data.Entity;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.API.Transactions;
using Unicasa.Domain.Interfaces.Repositories;
using Unicasa.Domain.Interfaces.Repositories.Base;
using Unicasa.Domain.Interfaces.Services;
using Unicasa.Domain.Services;
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

            //Serviço de Domain
            //container.RegisterType(typeof(IServiceBase<,>), typeof(ServiceBase<,>));

            container.RegisterType<IImportacaoService, ImportacaoService>(new HierarchicalLifetimeManager());

            //Repository
            container.RegisterType(typeof(IRepository<,>), typeof(Repository<,>));

            container.RegisterType<IImportacaoRepository, RepositoryImportacao>(new HierarchicalLifetimeManager());
        }
    }
}