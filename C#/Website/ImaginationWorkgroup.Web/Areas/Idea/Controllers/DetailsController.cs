using CoreServices.Api;
using CoreServices.Common;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Services;
using System.Linq;
using ImaginationWorkgroup.Web.Utilities;
using System;
using System.Web.Mvc;
using ImaginationWorkgroup.Infrastructure.Models;

namespace ImaginationWorkgroup.Web.Areas.Idea.Controllers
{
    public class DetailsController : Controller
    {
        IIdeaService _theService { get; }
        IEmailService _theEmailService { get; }
        IReviewerService _theReviewService { get; }
        public DetailsController(IIdeaService theService, IEmailService theEmailService, IReviewerService theReviewSerice)
        {
            _theService = theService;
            _theEmailService = theEmailService;
            _theReviewService = theReviewSerice;
        }

        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "List", new { area = "Idea"});
            }

            var theIdea = _theService.GetIdea((int)id);
            var info = UserInfoFactory.GetUserInfo(Request);

            //Only reviewers, employee, and supervisor can add comments.
            //Only reviwers that are in a certain review group and location can actually review the idea.
            //If the idea is pending review, only said reviers and employee/supervisor can view.

            bool canAddComments = false;
            bool canReviewTicket = false;
            bool isReviewer = User.IsInRole(RoleNames.Reviewer);
            
            var userElevatedAccess = _theReviewService.CheckElevatedAccess(theIdea.Id, info.Pin);
            var theReviewerGroups = _theReviewService.GetReviewGroupsForUser(info.Pin);

            //Someone can "review" an idea if they are both in the Reviewer application role and a member of the review group for the given status

            if(userElevatedAccess.Contains(ElevatedAccessLevel.Reviewer) && isReviewer)
            {
                canReviewTicket = true;
                canAddComments = true;
            }

            if(userElevatedAccess.Contains(ElevatedAccessLevel.User))
            {
                canAddComments = true;
            }

            if(theIdea.CurrentStatus.Status == "Implemented" || theIdea.CurrentStatus.Status == "Not Adopted")
            {
                //Don't add comments once the idea is implemented and not adopted. As per the 'bugs' that were opened.
                canAddComments = false;
            }

            if (theIdea.CurrentStatus.Status == "Pending Review")
            {
                //redirect if the user is not the employee, supervisor
                
                if (!userElevatedAccess.Any() || 
                    (!userElevatedAccess.Contains(ElevatedAccessLevel.User) 
                    && userElevatedAccess.Contains(ElevatedAccessLevel.Reviewer) 
                    && !isReviewer))
                {
                    return RedirectToAction("Index", "List", new { area = "Idea" });
                }
            }

            var theModel = _theService.ConvertIdeaToDetailsModel(theIdea);
            theModel.RenderCommentButton = canAddComments;

            theModel.CanReviewIdea = canReviewTicket;

            return View(theModel);

        }
        [RefreshDetectFilter]
        public ActionResult ReviewComplete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "List", new { area = "Idea" });
            }

            var theIdea = _theService.GetIdea((int)id);

            ViewBag.Id = id;

            switch(theIdea.CurrentStatus.Status)
            {
                case "Component Review":
                    ViewBag.Text = "You have approved this idea for display. The component will be notified automatically that the idea is ready for their review.";
                    break;
                case "CASI Review":
                    ViewBag.Text = "You have approved this idea for review by CASI. CASI will be notified automatically that the idea is ready for their review.";
                    break;
                case "Cadre Review":
                    ViewBag.Text = "You have approved this idea for review by the cadre. The cadre will be notified automatically that the idea is ready for their review.";
                    break;
                case "In Development":
                    ViewBag.Text = "You have marked this idea as in development. The employee and supervisor will be notified automatically of this action.";
                    break;
                case "Testing":
                    ViewBag.Text = "You have marked this idea as in testing. The employee and supervisor will be notified automatically of this action.";
                    break;
                case "Implemented":
                    ViewBag.Text = "You have marked this idea as implemented. The employee and supervisor will be notified automatically of this action.";
                    break;
                case "Component Implementation":
                    ViewBag.Text = "You have adopted this idea for component implementation. We have forwarded this idea to members of the component review group, who will decide how to implement it. The employee and his or her supervisor will be notified automatically that the idea has moved on to this phase.";
                    break;
                case "Adopted":
                    ViewBag.Text = "You have marked this idea as adopted. The employee and supervisor will be notified automatically of this action.";
                    break;
                case "Rejected":
                    ViewBag.Text = "You have chosen not to approve this idea for display. The employee and his/her supervisor will be automatically notified that the idea has been denied. The idea submission will no longer be viewable on the IWG website. If you have denied this idea due to inappropriate or abusive content, please notify your immediate supervisor.";
                    break;
                case "Not Adopted":
                    ViewBag.Text = "You have chosen to not adopt this idea. The employee and supervisor will be notified automatically of this action.";
                    break;
                default:
                    break;
            }
            //if (RouteData.Values["IsRefreshed"].ToString() == "")
            //{
            //    int i = 9;
            //    // page has been refreshed.
            //}

            return View();
        }

        public ActionResult LocationChanged(int? id)
        {
            ViewBag.Id = id;

            var theIdea = _theService.GetIdea((int)id);
            
            ViewBag.Text = $"You have routed this idea for review by CASI {theIdea.WorkLocation.City}. CASI {theIdea.WorkLocation.City} will be notified automatically that the idea is ready for its review.";

            return View();
        }
    }

}