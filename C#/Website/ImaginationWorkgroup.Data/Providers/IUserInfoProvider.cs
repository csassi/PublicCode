using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationWorkgroup.Data.Entities;

namespace ImaginationWorkgroup.Data.Providers
{
    public interface IUserInfoProvider
    {
        UserInfo FromEmail(string email);
        UserInfo FromPin(string pin);
        IEnumerable<UserInfo> WildcardByName(string lastName);
        IEnumerable<UserInfo> WildcardByName(string lastName, string firstName);
        IEnumerable<UserInfo> WildcardByName(string lastName, string firstName, string email);
        IEnumerable<UserInfo> WildcardByEmail(string email);
        IEnumerable<UserInfo> GetGroupMembers(string group);
        [Obsolete("Do not use to get group membership due to cross-domain issues. Use IsUserInGroup instead to validate group membership")]
        IEnumerable<string> GetAdGroupMembership(string pin);
        bool IsUserInGroup(string pin, string group);
    }
}
