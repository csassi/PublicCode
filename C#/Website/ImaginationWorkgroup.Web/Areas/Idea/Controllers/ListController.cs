using ImaginationWorkgroup.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImaginationWorkgroup.Web.Areas.Idea.Controllers
{
    public class ListController : Controller
    {
        private readonly IFeatureFlagProvider _ffProvider;

        public ListController(IFeatureFlagProvider ffProvider)
        {
            _ffProvider = ffProvider;
        }
        // GET: Idea/List
        public ActionResult Index()
        {
            if (_ffProvider.ListIdeasView)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
        }
    }
}