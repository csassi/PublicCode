using CoreServices.Api;
using CoreServices.Common;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Exceptions;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using ImaginationWorkgroup.Infrastructure.Services;
using ImaginationWorkgroup.Web.Areas.Idea.Controllers;
using ImaginationWorkgroup.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImaginationWorkgroup.Web.Api
{
    public class IdeaReviewController : ApiController
    {
        public class ReviewParams
        {
            public int IdeaId { get; set; }
            public int SelectedMapId { get; set; }
            public string Comment { get; set; }
        }

        IIdeaService _theService { get; }
        IEmailService _theEmailService { get; }
        IReviewerService _theReviewSerice { get; }

        public IdeaReviewController(IIdeaService service, IEmailService theEmailService, IReviewerService theReviewSerice)
        {
            _theService = service;
            _theEmailService = theEmailService;
            _theReviewSerice = theReviewSerice;
        }


        [System.Web.Http.HttpPost]
        public IHttpActionResult IdeaReview(ReviewParams param)
        {

            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase request = context.Request;

            var theIdea = _theService.GetIdea(param.IdeaId);
            var info = UserInfoFactory.GetUserInfo(request);
            var defaultApiConfig = new ApiConfig(new ConfigHelper());

            EmployeeProfile theProfile = _theService.GetMostRecentProfile(info.Pin);

            try
            {
                _theService.RouteIdea(theIdea, theProfile, param.SelectedMapId, param.Comment);
            }
            catch(UnexpectedStateException ex)
            {
                return InternalServerError(ex);
            }
            catch(Exception ex)
            {
                this.Log().Error(ex.ToString());
                return InternalServerError();
            }

            String theStatus = theIdea.CurrentStatus.Status;
            int theId = theIdea.Id;

            var theReviewers = _theReviewSerice.GetEmployeesFromReviewGroupsOrComponents(theIdea.Id);

            var theViewReviewToSend = "";
            if (theIdea.CurrentStatus.Status == "Not Adopted")
            {
                EmailNotAdoptedModel theModel = new EmailNotAdoptedModel
                {
                    Id = theId,
                    Reason = param.Comment,
                    Title = theIdea.Title,
                    Name = theIdea.Employee.FirstName,
                    EmployeeName = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}"
                };
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailNotAdopted", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.Employee.Email, $"Imagination Workgroup - Your Idea #{theIdea.Id} has not been adopted");

                theModel.Name = theIdea.EmployeeSupervisor.FirstName;
                theModel.IsSupEmail = true;
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailNotAdopted", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.EmployeeSupervisor.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} has not been adopted");
            }
            else if (theIdea.CurrentStatus.Status == "Rejected")
            {
                EmailRejectedModel theModel = new EmailRejectedModel
                {
                    Id = theId,
                    Reason = param.Comment,
                    Title = theIdea.Title,
                    Name = theIdea.Employee.FirstName,
                    EmployeeName = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}"
                };
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailRejected", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.Employee.Email, $"Imagination Workgroup - Your Idea #{theIdea.Id} has been rejected");

                theModel.Name = theIdea.EmployeeSupervisor.FirstName;
                theModel.IsSupEmail = true;
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailRejected", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.EmployeeSupervisor.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} has been rejected");
            }
            else if (theIdea.CurrentStatus.Status == "In Development")
            {
                EmailAdoptedModel theModel = new EmailAdoptedModel
                {
                    Id = theId,
                    Title = theIdea.Title,
                    Name = theIdea.Employee.FirstName,
                    EmployeeName = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}"
                };

                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailAdopted", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.Employee.Email, $"Imagination Workgroup - Your Idea #{theIdea.Id} is in development");

                theModel.IsSupEmail = true;
                theModel.Name = theIdea.EmployeeSupervisor.FirstName;
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailAdopted", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.EmployeeSupervisor.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} is in development");

                if (theReviewers != null)
                {
                    foreach (var reviewer in theReviewers)
                    {
                        theModel.Name = reviewer.Display;
                        theModel.IsSupEmail = false;
                        theModel.IsAdminEmail = true;
                        theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailAdopted", theModel);
                        _theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} is in development");
                    }
                }

            }
            else if (theIdea.CurrentStatus.Status == "Component Implementation")
            {
                EmailImplementedModel theModel = new EmailImplementedModel
                {
                    Id = theId,
                    Title = theIdea.Title,
                    Name = theIdea.Employee.FirstName,
                    EmployeeName = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}"
                };
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailComponentImplemented", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.Employee.Email, $"Imagination Workgroup - Your Idea #{theIdea.Id} has been adopted for Component Implementation");

                theModel.Name = theIdea.EmployeeSupervisor.FirstName;
                theModel.IsAdminEmail = false;
                theModel.IsSupEmail = true;
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailComponentImplemented", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.EmployeeSupervisor.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} has been adopted for Component Implementation");

                if (theReviewers != null)
                {
                    foreach (var reviewer in theReviewers)
                    {
                        theModel.Name = reviewer.Display;
                        theModel.IsAdminEmail = true;
                        theModel.IsSupEmail = false;

                        theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailComponentImplemented", theModel);
                        _theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} has been adopted for Component Implementation");
                    }
                }
            }
            else if (theIdea.CurrentStatus.Status == "Implemented")
            {
                EmailImplementedModel theModel = new EmailImplementedModel
                {
                    Id = theId,
                    Title = theIdea.Title,
                    Name = theIdea.Employee.FirstName,
                    EmployeeName = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}"
                };
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailImplemented", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.Employee.Email, $"Imagination Workgroup - Your Idea #{theIdea.Id} has been implemented");

                theModel.Name = theIdea.EmployeeSupervisor.FirstName;
                theModel.IsAdminEmail = false;
                theModel.IsSupEmail = true;
                theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailImplemented", theModel);
                _theEmailService.EmailIdea(theViewReviewToSend, theIdea.EmployeeSupervisor.Email, $"Imagination Workgroup - The Idea #{theIdea.Id} has been implemented");

                if (theReviewers != null)
                {
                    foreach (var reviewer in theReviewers)
                    {
                        theModel.Name = reviewer.Display;
                        theModel.IsAdminEmail = true;
                        theModel.IsSupEmail = false;

                        theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailImplemented", theModel);
                        _theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - The Idea #{theIdea.Id}  has been implemented");
                    }
                }
            }
            else
            {
                if (theReviewers != null)
                {
                    foreach (var reviewer in theReviewers)
                    {
                        EmailReviewerModel theModel = new EmailReviewerModel();
                        theModel.Id = theId;
                        theModel.Status = theStatus;
                        theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailReviewer", theModel);
                        _theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - New Idea #{theId} is ready for {theStatus}");
                    }
                }
            }

            String theReturn = "true";
            var response = new ServiceResponse(theReturn);
            return Json(response);
        }
    }
}
