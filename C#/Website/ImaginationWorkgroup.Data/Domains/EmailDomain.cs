using ImaginationWorkgroup.Data.Entities.Email;
using ImaginationWorkgroup.Data.Repositories;
using System;
using System.Configuration;
using System.Linq;

namespace ImaginationWorkgroup.Data.Domains
{
    public class EmailDomain : IEmailDomain
    {
        private IRepository _repo;

        private static string _from;

        static EmailDomain()
        {
            _from = ConfigurationManager.AppSettings["EmailFrom"];
        }
        public EmailDomain(IRepository repo)
        {
            _repo = repo;
        }

        public void AddEmail(string subject, string body, string[] to)
        {
            if (to == null || !to.Any())
                throw new ArgumentException("Addressees are required to create a new email", nameof(to));

            var header = new EmailHeader()
            {
                Body = body,
                Subject = subject,
                EmailFrom = _from,
                IsBodyHtml = true,
                Priority = 0,
                Sent = false,
                System = "Imagination"
            };

            _repo.Add(header);
            foreach (var addr in to)
            {
                var emailTo = new EmailAddressee()
                {
                    Email = header,
                    Addressee = addr
                };
                _repo.Add(emailTo);
            }
        }

        public void AddEmail(string subject, string body, string to)
        {
            AddEmail(subject, body, new[] { to });
        }
    }
}
