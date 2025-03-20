using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class ReviewerInfo
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Email { get; set; }
        public string Display => $"{First} {Last}";

        public ReviewerInfo()
        {

        }

        public ReviewerInfo(string first, string last, string email)
        {
            First = first;
            Last = last;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            var info = obj as ReviewerInfo;
            return info != null &&
                   First == info.First &&
                   Last == info.Last &&
                   Email == info.Email;
        }

        public override int GetHashCode()
        {
            var hashCode = -1603668999;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(First);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Last);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            return hashCode;
        }
    }
}
