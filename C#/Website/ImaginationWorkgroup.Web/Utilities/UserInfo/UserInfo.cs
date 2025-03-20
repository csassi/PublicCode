using System;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
//using AIRSflow.Keyer.Web.Models.Classes.Authorization;

namespace AIRSflow.Keyer.Web.Models
{
    /// <summary>
    /// This class is purposely not public. It should only
    /// be instantiated internally by a factory, NOT directly in code.
    /// </summary>
    class UserInfo : IUserInfo
    {
        HttpRequestBase RequestBase = null;
        HttpRequest Request = null;
        public UserInfo(HttpRequestBase request)
        {
            RequestBase = request;
        }
        public UserInfo(HttpRequest request)
        {
            Request = request;
        }
        private UserPrincipal usr = null;
        private UserPrincipal User
        {
            get
            {
                if (usr == null)
                    usr = getUserPrincipal();
                return usr;
            }
        }
        public string FirstName
        {
            get
            {
                return User == null || User.GivenName == null ? string.Empty : User.GivenName.ToString();
            }
        }
        public string LastName
        {
            get
            {
                return User == null || User.Surname == null ? string.Empty : User.Surname.ToString();
            }
        }
        private string _domain = string.Empty;
        public string Domain
        {
            get
            {
                return _domain;
            }
        }
        public string Pin
        {
            get
            {
                return User == null || User.SamAccountName == null ? string.Empty : User.SamAccountName.ToString();
            }
        }
        public string Email
        {
            get
            {
                return User == null || User.EmailAddress == null ? string.Empty : User.EmailAddress.ToString();
            }
        }
        public string DisplayName
        {
            get
            {
                return User == null || User.DisplayName == null ? string.Empty : User.DisplayName.ToString();
            }
        }
        private UserPrincipal getUserPrincipal()
        {
            try
            {

                string[] a = getLoginUserIdentity().Name.ToString().Split('\\');
                _domain = a[0].Trim();
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, this.Domain);
                UserPrincipal tmpusr = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, a[1]);
                ctx.Dispose();
                return tmpusr;
            }
            catch { }
            return null;
        }
        private WindowsIdentity getLoginUserIdentity()
        {
            if (RequestBase != null)
                return RequestBase.LogonUserIdentity;
            else
                return Request.LogonUserIdentity;
        }
    }
}