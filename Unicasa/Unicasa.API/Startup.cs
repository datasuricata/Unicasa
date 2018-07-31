using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using Unicasa.Api.Startups;
using Unicasa.API.DI;
using Unicasa.API.Security;
using Unity;

namespace Unicasa.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            var container = new UnityContainer();

            //TODO VALIDAR CULTUREINFO
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            //DependencyResolver.Resolve(container);

            DIStartup.ConfigureDependencyInjection(config, container);
            WebApiStartup.ConfigureWebApi(config);

            ConfigureOAuth(app, container);
            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new AuthorizationProvider(container)
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
