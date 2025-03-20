using System.Web;
using System.Web.Mvc;

namespace ImaginationWorkgroup.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RefreshDetectFilter());
        }
    }
}

public class RefreshDetectFilter : ActionFilterAttribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var cookie = filterContext.HttpContext.Request.Cookies["RefreshFilter"];
        filterContext.RouteData.Values["IsRefreshed"] = cookie != null &&
                                                        cookie.Value == filterContext.HttpContext.Request.Url.ToString();
    }
    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
        filterContext.HttpContext.Response.SetCookie(new HttpCookie("RefreshFilter", filterContext.HttpContext.Request.Url.ToString()));
    }
}