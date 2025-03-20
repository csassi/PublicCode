using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Services;
using ImaginationWorkgroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreServices.Api;
using CoreServices.Common;
using ImaginationWorkgroup.Data.Providers;
using ImaginationWorkgroup.Web.Utilities;

namespace ImaginationWorkgroup.Web.Areas.Idea.Controllers
{
    public class CreateController : Controller
    {
        IIdeaService theService { get; }
        IEmailService theEmailService { get; }
        IUserInfoProvider theUserInfoService { get; }
        IReviewerService theReviewService { get; }
        IFeatureFlagProvider _ffProvider { get; }
        public CreateController(IIdeaService _theService, IFeatureFlagProvider ffProvider, IEmailService _theEmailService, IReviewerService _theReviewService,IUserInfoProvider _theUserInfo)
        {
            theService = _theService;
            theEmailService = _theEmailService;
            _ffProvider = ffProvider;
            theUserInfoService = _theUserInfo;
            theReviewService = _theReviewService;
        }
        // GET: Idea/Create
        public ActionResult Index()
        {
            var theModel = new CreateIdeaSubmissionModel();
            theModel.Components = theService.GetComponents().ToList();
            theModel.Locations = theService.GetLocations().ToList();

            var theDescriptions = theService.GetAllDescriptionTypes();

            foreach (var item in theDescriptions)
            {
                switch(item.Name)
                {
                    case "Problem":
                        theModel.IdeaProblemPlaceholder = item.Placeholder;
                        break;
                    case "Idea":
                        theModel.IdeaTextPlaceholder = item.Placeholder;
                        break;
                    case "Title":
                        theModel.IdeaTitlePlaceholder = item.Placeholder;
                        break;
                    case "Benefits":
                        theModel.IdeaBenefitsPlaceholder = item.Placeholder;
                        break;
                }
            }

            if (_ffProvider.CreateIdeaView)
            {
                return View(theModel);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
            
        }

        [HttpGet]
        public ActionResult ThankYou(int theIdeaId)
        {
            ViewBag.Id = theIdeaId;
            return View();
        }
        [ValidateInput(false)]
        public ActionResult New(CreateIdeaSubmissionModel theModel)
        {
            var info = UserInfoFactory.GetUserInfo(Request);
            var defaultApiConfig = new ApiConfig(new ConfigHelper());
            var theIvfProxy = new IvfProxy(defaultApiConfig);
            var theIvf = theIvfProxy.GetByPin(info.Pin);
            var theSupAD = theUserInfoService.FromEmail(theModel.Supervisor);
            var theSupIvf = theIvfProxy.GetByPin(theSupAD.UserPin);
            var theIdeaStatus = theService.GetIdeaStatuses().Where(x => x.Status == "Pending Review").FirstOrDefault();
            var theProfile = theService.GetMostRecentProfile(info.Pin);

            EmployeeProfile theSupProfile = new EmployeeProfile()
            {
                Created = DateTimeOffset.Now,
                Modified = DateTimeOffset.Now,
                FirstName = theSupAD.First,
                LastName = theSupAD.Last,
                UserPin = theSupIvf.Pin,
                Mod = theSupIvf.Mod,
                Office = theSupIvf.OfficeCode,
                Position = theSupIvf.PositionCode,
                Email = theSupAD.Email
            };

            var theSameSupProfile = theService.CheckProfile(theSupProfile);
            if (theSameSupProfile == null)
            {
                theService.CreateProfile(theSupProfile);
            }
            else
            {
                theSupProfile = theSameSupProfile;
            }

            var theComponent = theService.GetComponent(theModel.UserComponent);
            var theLocation = theService.GetLocation(theModel.UserLocation);

            theModel.IdeaTitle = theModel.IdeaTitle.Replace("\r\n", "\n");//Stupid new line characters..../r/n needs to be /n because JavaScript is /n

            Data.Entities.Idea theNewIdea = new Data.Entities.Idea()
            {
                Created = DateTimeOffset.Now,
                Modified = DateTimeOffset.Now,
                Title = theModel.IdeaTitle,
                Component = theComponent,
                Employee = theProfile,
                EmployeeSupervisor = theSupProfile,
                CurrentStatus = theIdeaStatus,
                Location = theLocation,
                WorkLocation = theLocation
            };
            theService.CreateIdea(theNewIdea);

            var theDescriptionTypes = theService.GetAllDescriptionTypes();

            foreach (var type in theDescriptionTypes)
            {
                string theValue = "";
                if (type.Name == "Problem")
                {
                    theValue = theModel.IdeaProblem;
                }
                else if (type.Name == "Idea")
                {
                    theValue = theModel.IdeaText;
                }
                else if (type.Name == "Benefits")
                {
                    theValue = theModel.IdeaBenefits;
                }
                else
                {
                    continue;
                }

                theValue = theValue.Replace("\r\n", "\n");//Stupid new line characters..../r/n needs to be /n because JavaScript is /n
                IdeaDescription theIdeaDescription = new IdeaDescription()
                {
                    DescriptionType = type,
                    Created = DateTimeOffset.Now,
                    Modified = DateTimeOffset.Now,
                    DescriptionValue = theValue,
                    Idea = theNewIdea
                };
                theService.CreateDescription(theIdeaDescription);
            }
            
            ViewBag.Id = theNewIdea.Id;
            ViewBag.Name = theNewIdea.Employee.FirstName;
            ViewBag.FullName = $"{info.FirstName} {info.LastName}";

            //Employee Email
            var theViewToSend = ViewToString.ViewToHtml(this, "Email/_EmailEmployee", ViewBag);
            theEmailService.EmailIdea(theViewToSend, theProfile.Email, $"Thank you for submitting your idea #{theNewIdea.Id}!");
            
            //Supervisor Email
            theViewToSend = ViewToString.ViewToHtml(this, "Email/_EmailSupervisor", ViewBag);
            theEmailService.EmailIdea(theViewToSend, theSupProfile.Email, $"Idea #{theNewIdea.Id} has been submitted!");

            //Idea Police Email
            String theStatus = theNewIdea.CurrentStatus.Status;
            int theId = theNewIdea.Id;
            EmailModeratorModel theReviewModel = new EmailModeratorModel();
            theReviewModel.Id = theId;

            //Current status at this point is Pending Review
            var theReviewers = theReviewService.GetEmployeesFromReviewGroupsOrComponents(theNewIdea.Id);
            foreach (var reviewer in theReviewers)
            {
                var theViewReviewToSend = ViewToString.PartialRenderPartial("_EmailModerator", theReviewModel);
                theEmailService.EmailIdea(theViewReviewToSend, reviewer.Email, $"Imagination Workgroup - New Idea #{theId} is Pending Review");
            }

            return RedirectToAction("ThankYou", new { theIdeaId = theNewIdea.Id});
        }
    }
}