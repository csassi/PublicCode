using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        IEmailDomain _theDomain;
        public EmailService(IEmailDomain theDomain)
        {
            _theDomain = theDomain;
        }
        public void EmailIdea(String Body, String To, String Subject)
        {
            _theDomain.AddEmail(Subject, Body, To);
        }
    }
}
