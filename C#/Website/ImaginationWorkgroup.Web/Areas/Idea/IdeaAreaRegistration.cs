using System.Web.Mvc;

namespace ImaginationWorkgroup.Web.Areas.Idea
{
    public class IdeaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Idea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Idea_default",
                "Idea/{controller}/{action}/{id}/{theComment}",
                new { action = "Index", id = UrlParameter.Optional, theComment = UrlParameter.Optional }
            );
        }
    }
}