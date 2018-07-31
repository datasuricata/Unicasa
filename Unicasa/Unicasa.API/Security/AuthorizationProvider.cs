using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Unicasa.API.Persistence.Repositories;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;
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

                var request = new AutenticarRequest();

                request.Email = context.UserName;
                request.Senha = context.Password;

                if (request == null)
                {
                    context.SetError("invalid_grant", "Dados invalidos!");
                    return;
                }

                var usuario = new Usuario(request.Email, request.Senha);

                usuario = repository.ObterPor(x => x.Email == usuario.Email && x.Senha == usuario.Senha);

                if (usuario == null)
                {
                    context.SetError("invalid_grant", "Usuario não encontrado!");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //Definindo as Claims
                identity.AddClaim(new Claim("Usuario", JsonConvert.SerializeObject(usuario)));

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