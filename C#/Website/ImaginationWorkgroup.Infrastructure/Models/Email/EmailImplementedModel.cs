using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class EmailImplementedModel
    {
        public EmailImplementedModel()
        {
            IsAdminEmail = false;
            IsSupEmail = false;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string EmployeeName { get; set; }
        public bool IsAdminEmail { get; set; }
        public bool IsSupEmail { get; set; }
    }
}
