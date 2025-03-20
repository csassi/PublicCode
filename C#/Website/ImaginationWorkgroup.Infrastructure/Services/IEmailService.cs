using ImaginationWorkgroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public interface IEmailService
    {
        void EmailIdea(String Body, String To, String Subject);
    }
}
