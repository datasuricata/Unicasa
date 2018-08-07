using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Security;
using Unicasa.API.Transactions;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/usuario/conta")]
    public class ContaController : ControllerBase
    {
        private readonly RepositoryUsuario repository;
        private readonly UnicasaContext context;

        public ContaController()
        {
            context = new UnicasaContext();
            repository = new RepositoryUsuario(context);
            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [HttpPost]
        [Route("getToken")]
        public async Task<HttpResponseMessage> GenerateToken(AutenticarRequest request)
        {
            if (!Request.RequestUri.AbsoluteUri.Contains("localhost:51365"))
            {
                return await ResponseAsync(HttpStatusCode.BadRequest);
            }

            var usuario = new Usuario(request.Email, request.Senha);

            HttpClient _client = new HttpClient();
            _client.BaseAddress = Request.RequestUri;
            _client.Timeout = TimeSpan.FromMinutes(30);
            string requestUri = "/token";

            var auth = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("username", usuario.Email));
            keyValues.Add(new KeyValuePair<string, string>("password", usuario.Senha));

            auth.Content = new FormUrlEncodedContent(keyValues);

            var response = await _client.SendAsync(auth);
            var retorno = await response.Content.ReadAsStringAsync();

            var obj = new { access_token = "" };
            obj = JsonConvert.DeserializeAnonymousType(retorno, obj);

            return await ResponseAsync(new TokenClass(obj.access_token));
        }

        [Route("login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login(AutenticarRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var usuario = new Usuario(request.Email, request.Senha);

                usuario = repository.ObterPor(x => x.Email == usuario.Email && x.Senha == usuario.Senha);

                if(usuario == null)
                {
                    Notification.Add("Usuario não encontrado");
                    return null;
                }

                var response = new AutenticarResponse()
                {
                    Id = usuario.Id,
                    Perfil = usuario.UserRole,
                    Message = "Autorizado: " + usuario.NomeCompleto,
                    Nome = usuario.NomeCompleto,
                    Email = usuario.Email
                };

                HttpClient _client = new HttpClient();
                _client.BaseAddress = Request.RequestUri;
                _client.Timeout = TimeSpan.FromMinutes(30);
                string requestUri = "/token";

                var auth = new HttpRequestMessage(HttpMethod.Post, requestUri);

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
                keyValues.Add(new KeyValuePair<string, string>("username", usuario.Email));
                keyValues.Add(new KeyValuePair<string, string>("password", usuario.Senha));

                auth.Content = new FormUrlEncodedContent(keyValues);

                var autenticar = await _client.SendAsync(auth);

                var retorno = await autenticar.Content.ReadAsStringAsync();

                var obj = new { access_token = "" };
                obj = JsonConvert.DeserializeAnonymousType(retorno, obj);

                //return await ResponseAsync(new TokenClass(obj.access_token));

                response.Token = obj.access_token;

                return Request.CreateResponse(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }

    internal class TokenClass
    {
        public TokenClass(string token)
        {
            access_token = "bearer " + token;
        }
        public string access_token { get; set; }
    }
}