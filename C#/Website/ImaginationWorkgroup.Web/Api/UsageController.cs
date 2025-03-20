using CoreServices.Api;
using CoreServices.Common;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using ImaginationWorkgroup.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    public class UsageController : ApiController
    {
        private IUsageService _service;
        private IIdeaService _theIdaservice;
        public UsageController(IUsageService service, IIdeaService ideaService)
        {
            _service = service;
            _theIdaservice = ideaService;
        }
        [HttpPost]
        public IHttpActionResult PostUsage(UsageData data)
        {

            //Profile Stuff. Best place to piggy back. 
            var context = new HttpContextWrapper(HttpContext.Current);
            HttpRequestBase request = context.Request;
            
            var info = UserInfoFactory.GetUserInfo(request);
            var defaultApiConfig = new ApiConfig(new ConfigHelper());
            var theIvfProxy = new IvfProxy(defaultApiConfig);
            var theIvf = theIvfProxy.GetByPin(info.Pin);

            EmployeeProfile theProfile = new EmployeeProfile()
            {
                Created = DateTimeOffset.Now,
                Modified = DateTimeOffset.Now,
                FirstName = info.FirstName,
                LastName = info.LastName,
                UserPin = info.Pin,
                Mod = theIvf.Mod,
                Office = theIvf.OfficeCode,
                Position = theIvf.PositionCode,
                Email = info.Email
            };

            var theSameProfile = _theIdaservice.CheckProfile(theProfile);
            if (theSameProfile == null)
            {
                _theIdaservice.CreateProfile(theProfile);
            }

            _service.RecordUsage(data, User.Identity.Name.Split('\\').Last());
            return Json(true);
        }
    }
}
