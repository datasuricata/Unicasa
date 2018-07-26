using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Unicasa.Domain.Arguments;

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


                var request = new AutenticarRequest();

                request.Email = context.UserName;
                request.Senha = context.Password;

                //var response = serviceUsuario.AutenticarUsuario(request);

                //if (serviceUsuario.IsInvalid())
                //{
                //    if (response == null)
                //    {
                //        context.SetError("invalid_grant", "Preencha um e-mail válido e uma senha com pelo menos 6 caracteres.");
                //        return;
                //    }
                //}

                //if (response == null)
                //{
                //    context.SetError("invalid_grant", "Usuario não encontrado!");
                //    return;
                //}

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //Definindo as Claims
               // identity.AddClaim(new Claim("Usuario", JsonConvert.SerializeObject(response)));

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