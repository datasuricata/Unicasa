using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Transactions;
using Unicasa.Domain.Interfaces.Services.Base;

namespace Unicasa.API.Controllers.Base
{
    public class ControllerBase : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IBaseService _serviceBase;

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HttpResponseMessage> ResponseAsync(object result, IBaseService serviceBase)
        {
            _serviceBase = serviceBase;

            if (!serviceBase.Notifications().Any())
            {
                try
                {
                    _unitOfWork.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, $"Houve um problema interno com o servidor. Entre em contato com o Administrador do sistema caso o problema persista. Erro interno: {ex.Message}");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = serviceBase.Notifications()});
            }
        }

        public async Task<HttpResponseMessage> ResponseExceptionAsync(Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { errors = ex.Message, exception = ex.ToString() });
        }

        protected override void Dispose(bool disposing)
        {
            if (_serviceBase != null)
            {
                _serviceBase.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}