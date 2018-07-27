using System.Web;
using System.Web.Http;
using Unicasa.Api.Startups;

namespace Unicasa.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiStartup.ConfigureWebApi);
        }
    }
}
