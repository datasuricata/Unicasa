using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicasa.Domain.Helper;
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

        protected async Task<T> Get<T>(string Uri)
        {
            GetToken();
            var request = new DataRequest<T>();
            var result = await request.Get(Uri, Token);

            return result;
        }
        protected async Task<T> Post<T>(string Uri, object command)
        {
            GetToken();
            var request = new DataRequest<T>();
            var result = await request.Post(Uri, command, Token);

            return result;
        }
        protected async Task PostAnonymous<T>(string Uri, object command)
        {
            GetToken();
            var request = new DataRequest<T>();
            await request.PostAnonymous(Uri, command, Token);

        }
        protected async Task<T> GetById<T>(string Uri, string id)
        {
            GetToken();
            var request = new DataRequest<T>();
            var result = await request.GetById(Uri, id, Token);

            return result;
        }
        protected async Task<T> Put<T>(string Uri, object command)
        {
            GetToken();
            var request = new DataRequest<T>();
            var result = await request.Put(Uri, command, Token);

            return result;
        }

        #endregion

        #region [ MESSAGES ]

        protected void SetError(string message)
        {
            TempData["Message"] = message;
        }

        protected void SetSuccess(string message)
        {
            TempData["MessageSuccess"] = message;
        }

        protected class returnJs
        {
            public int StatusCode { get; set; }
            public string Mensagem { get; set; }
        }

        #endregion
    }
}