using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Services;
using ImaginationWorkgroup.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    public class IdeaController : ApiController
    {
        private IIdeaService _ideaService;
        private IReviewerService _reviewService;
        private IEmailService _emailService;

        public IdeaController(IIdeaService ideaService, IReviewerService theReviewSerice, IEmailService theEmailService)
        {
            _ideaService = ideaService;
            _reviewService = theReviewSerice;
            _emailService = theEmailService;
        }

        [HttpGet]
        public IHttpActionResult GetIdea(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("api/Idea/{id}", Name = "patch idea")]
        public IHttpActionResult UpdateIdea([FromBody] IdeaPatchModel patchModel, int id)
        {

            if (patchModel == null)
            {
                return BadRequest("You must provide items to update");
            }

            var idea = _ideaService.GetIdea(id);
            var previousWorkLocationCity = idea.WorkLocation.City;

            _ideaService.UpdateIdeaWorkLocation(id, patchModel.WorkLocationId);
            var newWorkLocationCity = idea.WorkLocation.City;

            var theReviewers = _reviewService.GetEmployeesFromReviewGroupsOrComponents(id);

            if (theReviewers != null)
            {
                foreach (var reviewer in theReviewers)
                {
                    EmailChangeLocation theModel = new EmailChangeLocation();
                    theModel.Id = id;
                    theModel.FromLocation = previousWorkLocationCity;
                    theModel.ToLocation = newWorkLocationCity;
                    var theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailLocationChanged", theModel);
                    _emailService.EmailIdea(theViewReviewToSend, reviewer.Email, $" Imagination Workgroup – Idea #{id} has changed CASI location");
                }
            }
            
            return Ok();
        }
    }

    public class IdeaPatchModel
    {
        public int WorkLocationId { get; set; }
    }
}
