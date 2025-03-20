using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    [Authorize]
    public class IdeaFilterController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetFilters()
        {
            var filters = new List<IdeaFilter>()
            {
                new IdeaFilter(FilterType.Open, "Open Ideas", "View all ideas that are open (In Review + In Progress) in the system"),
                new IdeaFilter(FilterType.InReview, "In Review", "View all ideas that are current in a review state"),
                new IdeaFilter(FilterType.InProgress, "In Progress", "View all ideas that have been approved and are being worked on"),
                new IdeaFilter(FilterType.Implemented, "Implemented", "View all ideas that have been adopted and implemented"),
                new IdeaFilter(FilterType.NotAdopted, "Not Adopted", "View all submitted ideas that were not adopted"),
                new IdeaFilter(FilterType.MyIdeas, "My Ideas", "View ideas that I submitted")
            };

            if (User.IsInRole(RoleNames.Reviewer))
            {
                filters.Add(new IdeaFilter(FilterType.MyWork, "My Work", "View ideas that I am a reviewer for"));
            }

            //TODO: if reviewer add the MyReview section
            return Json(new ServiceResponse(filters));
        }
    }
}

