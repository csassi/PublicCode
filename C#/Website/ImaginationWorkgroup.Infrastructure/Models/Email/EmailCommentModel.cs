using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class EmailCommentModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string When { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
