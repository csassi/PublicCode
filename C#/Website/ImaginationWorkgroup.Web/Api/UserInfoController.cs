using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImaginationWorkgroup.Web.Api
{
    public class UserInfoController : ApiController
    {
        private IUserInfoProvider _userProvider;

        public UserInfoController(IUserInfoProvider employeeProvider)
        {
            _userProvider = employeeProvider;
        }


        /// <summary>
        /// Search by name for a user. The name provided is expected to be in the format of last,first and partials are allowed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Request.CreateResponse(new { Employees = new { } });
            var splitNames = name.Split(new[] { ',' });
            var last = splitNames[0].Trim();
            string first = null;
            if (splitNames.Length > 1)
            {
                first = splitNames[1].Trim();
            }
            var results = _userProvider.WildcardByName(last, first);
            return Request.CreateResponse(new { Employees = results });

        }

        [HttpGet]
        public HttpResponseMessage ByPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            //strip all non-numeric values from the pin that we are searching for. 
            var strippedPin = new string(pin.Where(c => char.IsDigit(c)).ToArray());
            var employee = _userProvider.FromPin(strippedPin);
            if (employee == UserInfo.NotFound)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(new { Employee = employee });
        }
        [HttpGet]
        public HttpResponseMessage ByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            var employee = _userProvider.FromEmail(email);
            if (employee == UserInfo.NotFound)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(new { Employee = employee });
        }
    }
}
