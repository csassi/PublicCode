using ImaginationWorkgroup.Data.Providers;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using ImaginationWorkgroup.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    public class IdeaListingController : ApiController
    {
        private IIdeaService _service;
        private IReviewerService _reviewerService;
        private IFeatureFlagProvider _ffProvider;

        public IdeaListingController(IIdeaService service, IReviewerService reviewerService, IFeatureFlagProvider ffProvider)
        {
            _service = service;
            _reviewerService = reviewerService;
            _ffProvider = ffProvider;
        }

        [HttpGet]
        public IHttpActionResult GetListing(int? filter)
        {
            if (filter == null)
                return Json(new ServiceResponse(null, new List<ResponseMessage>() { new ResponseMessage("No filter provided") }, true));

            var filterType = (FilterType)filter;
            var listItems = _service.GetIdeas(filterType, User.Identity.Name.Split('\\').Last(), User.IsInRole(RoleNames.Reviewer));
            var model = new IdeaListModel(listItems, User.IsInRole(RoleNames.Reviewer), _ffProvider.ViewIdeaDetails);
            var response = new ServiceResponse(model);
            return Json(response);
        }
    }
}
