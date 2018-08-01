using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Unicasa.Web.Filters
{
    public class AutorizeUser : ActionFilterAttribute, IActionFilter, IResultFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filtercontext)
        {

            //[Validando Sessão] - ActionExecuting - Filter Active
            if (HttpContext.Current.Session["AuthorizedUserId"] == null)
            {
                filtercontext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "UsuarioConta" }, { "Action", "Login" } });
            }

            base.OnActionExecuting(filtercontext);
        }
    }
}