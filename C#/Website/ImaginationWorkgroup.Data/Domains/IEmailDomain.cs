using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Domains
{
    public interface IEmailDomain
    {
        void AddEmail(string subject, string body, string[] to);
        void AddEmail(string subject, string body, string to);
    }
}
