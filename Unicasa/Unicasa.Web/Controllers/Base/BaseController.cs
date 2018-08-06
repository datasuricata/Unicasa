using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Helper;
using Unicasa.Web.Helpers.Exceptions;
using Unicasa.Web.Requests;

namespace Unicasa.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        #region [ PARAMETROS ]

        protected Components Components;
        protected string Token;
        protected List<string> Notifications;

        #endregion

        #region [ CTOR ]

        public BaseController()
        {
            Notifications = new List<string>();
            Components = new Components();
        }

        #endregion

        #region [ UTEIS ]

        protected ActionResult GetToken()
        {
            Token = null;
            if (Session["AuthorizedUserId"] != null)
            {
                Token = HttpContext.Session["access_token"].ToString();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
            return View();
        }
        protected string DropDownToJson(List<GenericCheckBoxListItem> request)
        {
            var lista = new Dictionary<string, string>();

            foreach (var item in request)
            {
                lista.Add(item.Id, item.Display);
            }

            return JsonConvert.SerializeObject(lista);
        }

        #endregion

        #region [ TASKS ASYNC ]

        protected async Task PostAnonymous<T>(string Uri, object command)
        {
            GetToken();
            var request = new DataRequest<T>();
            await request.PostAnonymous(Uri, command, Token);

        }

        protected async Task<T> Get<T>(string Uri)
        {
            var request = new DataRequest<T>();
            try
            {
                GetToken();
                var result = await request.Get(Uri, Token);
                StoreErrors(request);
                return result;
            }
            catch (ApiException e)
            {
                SetError(e.Message);
                return default(T);
            }
        }
        protected async Task<T> Post<T>(string Uri, object command)
        {
            var request = new DataRequest<T>();
            try
            {
                GetToken();
                var result = await request.Post(Uri, command, Token);

                StoreErrors(request);

                return result;
            }
            catch (ApiException e)
            {
                SetError(e.Message);
                return default(T);
            }
        }
        protected async Task<T> GetById<T>(string Uri, string id)
        {
            var request = new DataRequest<T>();
            try
            {
                GetToken();
                var result = await request.GetById(Uri, id, Token);
                StoreErrors(request);
                return result;
            }
            catch (ApiException e)
            {
                SetError(e.Message);
                return default(T);
            }
        }
        protected async Task<T> Put<T>(string Uri, object command)
        {
            var request = new DataRequest<T>();
            try
            {
                GetToken();
                var result = await request.Put(Uri, command, Token);

                StoreErrors(request);

                return result;
            }
            catch (ApiException e)
            {
                SetError(e.Message);
                return default(T);
            }
        }

        #endregion

        #region [ MESSAGES ]

        protected void StoreErrors<T>(DataRequest<T> request)
        {
            if (request.ErrosRequest != null)
            {
                BaseResponse response = JsonConvert.DeserializeObject<BaseResponse>(request.ErrosRequest);

                if (response.Exceptions.Any())
                {
                    List<string> Message = new List<string>();

                    foreach (var error in response.Exceptions)
                        Message.Add(error);

                    SetErrors(Message);
                }
                else
                    SetSuccess(response.Message);
            }
        }
        protected void SetError(string message)
        {
            TempData["Message"] = message;
        }
        protected void SetErrors(List<string> messages)
        {
            TempData["Messages"] = messages;
        }
        protected void SetSuccess(string message)
        {
            TempData["MessageSuccess"] = message;
        }

        #endregion
    }
}