using ImaginationWorkgroup.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImaginationWorkgroup.Web.Controllers
{
    public class HomeController : Controller
    {
        private IFeatureFlagProvider _provider;
        public HomeController(IFeatureFlagProvider provider)
        {
            _provider = provider;
        }
        public ActionResult Index()
        {
            if (_provider.RedirectToAbout)
            {
                return RedirectToAction("Index", "Background", new { area = "About" });
            }
            return View();
        }
       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}