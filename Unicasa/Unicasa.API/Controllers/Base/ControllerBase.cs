using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Transactions;

namespace Unicasa.API.Controllers.Base
{
    public class ControllerBase : ApiController
    {
        protected List<string> Notification;
        public UnitOfWork uow;

        public ControllerBase()
        {

        }

        public async Task<HttpResponseMessage> ResponseAsync(object result)
        {

            if (!Notification.Any())
            {
                try
                {
                    uow.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, $"Houve um problema interno com o servidor. Entre em contato com o Administrador do sistema caso o problema persista. Erro interno: {ex.Message}");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = Notification });
            }
        }

        public async Task<HttpResponseMessage> ResponseExceptionAsync(Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { errors = ex.Message, exception = ex.ToString() });
        }
    }
}