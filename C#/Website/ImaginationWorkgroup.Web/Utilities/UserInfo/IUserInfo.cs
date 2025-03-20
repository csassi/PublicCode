using System;
namespace AIRSflow.Keyer.Web.Models
{
    public interface IUserInfo
    {
        string DisplayName { get; }
        string Domain { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        string Pin { get; }
    }
}
