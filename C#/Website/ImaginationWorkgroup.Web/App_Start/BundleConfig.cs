using System.Web;
using System.Web.Optimization;

namespace ImaginationWorkgroup.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle(CustomBundles.JS.JQueryUI).Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle(CustomBundles.JS.Knockout).Include(
                    "~/Scripts/knockout-3.5.0.js",
                    "~/Scripts/knockout.validation.js",
                    "~/Scripts/KnockoutExtenstions/custom-handlers.js"
                ));

            bundles.Add(new ScriptBundle(CustomBundles.JS.IdeaDetails).Include(
                    "~/Scripts/Areas/Idea/Details/idea-details.js"
                    ));

            bundles.Add(new ScriptBundle(CustomBundles.JS.CreateIdea).Include(
                    "~/Scripts/Areas/Idea/Create/create-idea.js"
                ));
            bundles.Add(new ScriptBundle(CustomBundles.JS.ViewIdea).Include(
                    "~/Scripts/Areas/Idea/List/idea-listing.js"
                    ));
            bundles.Add(new ScriptBundle(CustomBundles.JS.Tablesorter).Include(
                    "~/Scripts/tablesorter/jquery.tablesorter.combined.min.js"
                    ));

        }
    }
    public class CustomBundles
    {
        public class JS
        {
            public static readonly string CreateIdea = "~/bundles/createIdea";
            public static readonly string ViewIdea = "~/bundles/viewIdea";
            public static readonly string IdeaDetails = "~/bundles/ideaDetails";
            public static readonly string Knockout = "~/bundles/knockout";
            public static readonly string JQueryUI = "~/bundles/jqueryUI";
            public static readonly string Tablesorter = "~/bundles/jqueryTablesorter";
        }

        public class CSS
        {

        }
    }
}
