using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Unicasa.Domain.Interfaces.Repositories;
using Unity;

namespace Unicasa.API.Security
{
    public class AuthorizationProvider : OAuthAuthorizationServerProvider
    {
        private readonly UnityContainer _container;

        public AuthorizationProvider(UnityContainer container)
        {
            _container = container;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                //IUsuarioService serviceUsuario = _container.Resolve<IUsuarioService>();
                IUsuarioRepository repository = _container.Resolve<IUsuarioRepository>();

                var request = repository.ObterPor(x => x.Email == context.UserName && x.Senha == context.Password);

                if (request == null)
                {
                    context.SetError("invalid_grant", "Usuario não encontrado!");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //Definindo as Claims
                identity.AddClaim(new Claim("Usuario", JsonConvert.SerializeObject(request)));

                var principal = new GenericPrincipal(identity, new string[] { });

                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }
        }
    }
}