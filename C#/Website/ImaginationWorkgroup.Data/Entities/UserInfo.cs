using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public class UserInfo
    {
        [StringLength(100)]
        public string First { get; set; }
        [StringLength(100)]
        public string Last { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(100)]
        public string OfficeInfo { get; set; }
        [StringLength(15)]
        public string UserPin { get; set; }
        public string PrimaryPhone { get; set; }
        public bool IsManagerial { get; set; }
        public string DisplayName => $"{First} {Last}";

        public UserInfo()
        {
        }

        public UserInfo(string first, string last, string email, string primaryPhone, string officeInfo, string userPin)
            : this(first, last, email, primaryPhone, officeInfo, userPin, false)
        {

        }
        public UserInfo(string first, string last, string email, string primaryPhone, string officeInfo, string userPin, bool isManagerial)
        {
            First = first;
            Last = last;
            Email = email;
            PrimaryPhone = primaryPhone;
            OfficeInfo = officeInfo;
            UserPin = userPin;
        }

        public static UserInfo NotFound = new UserInfo(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    }
}
