using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Helper;
using Unicasa.Web.Helpers.Exceptions;

namespace Unicasa.Web.Requests
{
    public class DataRequest<T> : BaseRequest
    {

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<BaseResponse>(content);
                var message = "";

                if (errors.Exceptions.Any())
                    message = errors.ToString();

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ApiException(message);

                else
                    throw new ApiException("Ocorreu um erro.");
            }
        }

        public async Task<T> Get(string endpoint, string token = "")
        {
            var response = await SendAsync(RequestMethod.Get, endpoint, null, token);

            var retorno = await response.Content.ReadAsStringAsync();

            //await HandleResponse(response);


            return JsonConvert.DeserializeObject<T>(retorno);

        }
        public async Task<T> Post(string endpoint, object command, string token = "")
        {
            var response = await SendAsync(RequestMethod.Post, endpoint, command, token);

            var retorno = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(retorno);

        }
        public async Task PostAnonymous(string endpoint, object command, string token = "")
        {
            var response = await SendAsync(RequestMethod.Post, endpoint, command, token);

            var retorno = await response.Content.ReadAsStringAsync();
        }
        public async Task<T> Put(string endpoint, object command, string token)
        {
            var response = await SendAsync(RequestMethod.Put, endpoint, command, token);

            var retorno = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(retorno);
        }
        public async Task<T> GetById(string endpoint, string id, string token = "")
        {
            var response = await SendAsync(RequestMethod.Get, $"{endpoint}?id={id}", null, token);

            var retorno = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(retorno);

        }

    }
}