using System;
using System.Web;
using AIRSflow.Keyer.Web.Models;

/// <summary>
/// This class is public and purposely not in a namespace.
/// It is global and can be used anywhere in application
/// </summary>
public class UserInfoFactory
{
    private static IUserInfo info = null;
    private static HttpRequestBase _requestBase = null;
    private static HttpRequest _request = null;
    /// <summary>
    /// This takes in an HttpRequestBase object that is generated
    /// for every MVC page request
    /// </summary>
    /// <param name="Request">HttpRequestBase object (for MVC applications)</param>
    /// <returns>Returns an instance of an implementation of an IUserInfo object</returns>
    public static IUserInfo GetUserInfo(HttpRequestBase Request)
    {
        if (info == null || _requestBase == null || Request != _requestBase)
        {//if it's the first time or a different request, reset user info
            _requestBase = Request;
            info = new UserInfo(Request);//*NOTE* You can create your own version of this class. Just implement the IUserInfo interface.
        }
        return info;
    }
    /// <summary>
    /// This takes in an HttpRequest object that is generated for every ASP.NET page
    /// </summary>
    /// <param name="Request">HttpRequest object (for ASP.NET applications)</param>
    /// <returns>Returns an instance of an implementation of an IUserInfo object</returns>
    public static IUserInfo GetUserInfo(HttpRequest Request)
    {
        if (info == null || _request == null || Request != _request)
        {//if it's the first time or a different request, reset user info
            _request = Request;
            info = new UserInfo(Request);//*NOTE* You can create your own version of this class. Just implement the IUserInfo interface.
        }
        return info;
    }
}
