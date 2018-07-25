using System.Web;
using System.Web.Http;

namespace Unicasa.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(Startup.Register);
        }
    }
}
