using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImaginationWorkgroup.Web.Utilities
{

    public class ViewToString
    {
        private class Email : ControllerBase
        {
            protected override void ExecuteCore() { }
        }

        static public string ViewToHtml(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }

        }
        public static string PartialRenderPartial(string partialName, object model)
        {
            var sw = new StringWriter();
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            // point to an empty controller
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Email");

            var controllerContext = new ControllerContext(new RequestContext(httpContext, routeData), new Email());

            var result = ViewEngines.Engines.FindPartialView(controllerContext, partialName);

            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialName).View;

            view.Render(new ViewContext(controllerContext, view, new ViewDataDictionary { Model = model }, new TempDataDictionary(), sw), sw);

            return sw.ToString();
        }

        public static string ViewToHtml(string controllerName, string viewName,
                                        object viewData)
        {
            HttpContextBase contextBase = new HttpContextWrapper(HttpContext.Current);

            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);
            var controllerContext = new ControllerContext(contextBase, routeData,
                                                           new Email());

            var razorViewEngine = new RazorViewEngine();
            var razorViewResult = razorViewEngine.FindView(controllerContext, viewName,
                                                             "", false);

            var writer = new StringWriter();
            var viewContext = new ViewContext(controllerContext, razorViewResult.View,
                   new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
            razorViewResult.View.Render(viewContext, writer);

            return writer.ToString();
        }
    }

}