using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Unicasa.Domain.Helper;

namespace Unicasa.Web.Requests
{
    public class BaseRequest
    {
        public BaseRequest()
        {
            if (HttpContext.Current.Request.Url.Host.ToLower().Contains("localhost"))
            {
                baseAddressUrl = "http://localhost:51365/";
            }
            else
            {
                baseAddressUrl = "http://api.granero.unicasa.scarattisolucoesemti.com.br";
            }
        }

        #region [ Atributos ]

        protected string baseAddressUrl;
        private MediaTypeWithQualityHeaderValue teste = new MediaTypeWithQualityHeaderValue("application/json");
        private HttpClient _client = new HttpClient();

        #endregion [ Atributos ]

        #region [ Propriedades ]

        public string ErrosRequest { get; set; }

        #endregion [ Propriedades ]

        #region [ SendAsync ]

        protected async Task<HttpResponseMessage> SendAsync(RequestMethod metodoRequisicao, string requestUri, object parametros = null, string token = "")
        {
            _client.BaseAddress = new Uri(baseAddressUrl);
            _client.Timeout = TimeSpan.FromMinutes(30);

            switch (metodoRequisicao)
            {
                case RequestMethod.Get:
                    {
                        if (!string.IsNullOrEmpty(token))
                            return await Get(requestUri, token);
                        return await Get(requestUri);
                    }
                case RequestMethod.Post:
                    {
                        if (!string.IsNullOrEmpty(token))
                            return await Post(requestUri, parametros, token);
                        return await Post(requestUri, parametros);
                    }
                case RequestMethod.Put:
                    {
                        if (!string.IsNullOrEmpty(token))
                            return await Put(requestUri, parametros, token);
                        return await Put(requestUri, parametros);
                    }
            }
            return null;
        }

        #endregion [ SendAsync ]

        #region [ GET ]

        private async Task<HttpResponseMessage> Get(string requestUri, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            return await _client.GetAsync(requestUri);
        }

        #endregion [ GET ]

        #region [ POST ]

        private async Task<HttpResponseMessage> Post(string requestUri, object parametros, string token = "")
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = JsonConvert.SerializeObject(parametros);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PostAsync(requestUri, content);
        }

        #endregion [ POST ]

        #region [ PUT ]

        private async Task<HttpResponseMessage> Put(string requestUri, object parametros, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = JsonConvert.SerializeObject(parametros);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _client.PutAsync(requestUri, content);
        }

        #endregion [ PUT ]

        #region [ Login with Token ]

        public async Task<OAuthCommand> LoginWithToken(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;
            return await GetUserToken(login, password);
        }

        #endregion [ Login with Token ]

        #region [ OAuth ]

        public async Task<OAuthCommand> GetUserToken(string login, string password)
        {
            try
            {
                _client.BaseAddress = new Uri(baseAddressUrl);
                _client.Timeout = TimeSpan.FromMinutes(30);
                string requestUri = "/api/security/token";

                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
                keyValues.Add(new KeyValuePair<string, string>("username", login));
                keyValues.Add(new KeyValuePair<string, string>("password", password));

                request.Content = new FormUrlEncodedContent(keyValues);

                var response = await _client.SendAsync(request);
                var retorno = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OAuthCommand>(retorno);
            }
            catch (Exception e)
            {
                return new OAuthCommand
                {
                    Message = e.Message
                };
            }
        }

        #endregion [ OAuth ]
    }

    #region [ OAuth Command ]

    public class OAuthCommand
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string Message { get; set; }
    }

    #endregion [ OAuth Command ]
}