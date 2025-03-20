using CoreServices.Api;
using CoreServices.Common;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using ImaginationWorkgroup.Infrastructure.Services;
using ImaginationWorkgroup.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    public class CommentController : ApiController
    {

        public class BallSackParams
        {
            public int id { get; set; }
            public string param { get; set; }
        }

        IIdeaService _theService;
        IEmailService _theEmailService;
        IReviewerService _theReviewService;

        public CommentController(IIdeaService service, IEmailService theEmailService, IReviewerService theReviewService)
        {
            _theService = service;
            _theEmailService = theEmailService;
            _theReviewService = theReviewService;
        }


        [HttpPost]
        public IHttpActionResult AddComment(BallSackParams param)
        {
            int theIdeaId = param.id;
            String theComment = param.param;
            var theIdea = _theService.GetIdea(theIdeaId);

            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase request = context.Request;

            var info = UserInfoFactory.GetUserInfo(request);
            var theProfile = _theService.GetMostRecentProfile(info.Pin);

            var theNewComment = new IdeaComment()
            {
                Comment = theComment,
                Employee = theProfile,
                Created = DateTimeOffset.Now,
                Modified = DateTimeOffset.Now,
                Idea = theIdea

            };
            _theService.CreateComment(theNewComment);

            String theStatus = theIdea.CurrentStatus.Status;
            int theId = theIdea.Id;

            EmailCommentModel theModel = new EmailCommentModel
            {
                Id = theId,
                Comment = theComment,
                Title = theIdea.Title,
                Name = info.FirstName,
                Status = theStatus,
                When = theNewComment.Created.DateTime.ToString("MM/dd/yyyy hh:mm tt")
            };
            
            //Only email when in these statuses.
            if (theStatus == "CASI Review" || theStatus == "Pending Review" || theStatus == "Component Review")
            {
                var theReviewers = _theReviewService.GetEmployeesFromReviewGroupsOrComponents(theIdea.Id);
                var theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailComment", theModel);
                if (theReviewers != null)
                {
                    foreach (var reviewer in theReviewers)
                    {
                        if (reviewer.Email.ToLower() == info.Email.ToLower())
                        {   //no need to email ourselves.
                            continue;
                        }
                        _theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - A new comment has been entered for Idea #{theIdea.Id}");
                    }
                }
            }

            String theReturn = "true";
            var response = new ServiceResponse(theReturn);
            return Json(response);
        }

        [HttpGet]
        public IHttpActionResult GetListing(int id)
        {
            var listItems = _theService.GetIdeaComments(id);
            var commentlist = new List<CommentListItem>();

            listItems = listItems.OrderByDescending(x => x.Created);

            foreach (var item in listItems)
            {
                var theNewListItem = new CommentListItem()
                {
                    Comment = item.Comment,
                    Created = item.Created.DateTime.ToString("MM/dd/yyyy hh:mm tt"),
                    Employee = item.Employee.FirstName + ' ' + item.Employee.LastName
                };

                commentlist.Add(theNewListItem);
            }

            var response = new ServiceResponse(commentlist);
            return Json(response);
        }
    }
}
