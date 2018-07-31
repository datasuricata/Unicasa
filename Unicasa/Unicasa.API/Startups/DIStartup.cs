using System.Web.Http;
using Unicasa.API.DI;
using Unity;

namespace Unicasa.Api.Startups
{
    public class DIStartup
    {
        public static void ConfigureDependencyInjection(HttpConfiguration config, UnityContainer container)
        {
            DependencyResolver.Resolve(container);
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}