using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class IdeaListItem : ViewModelBase
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Component { get; set; }
        public string Status { get; set; }
        public bool DisplayForEmployee { get; set; }
        public bool CanAccess { get; set; }
        public string Location { get; set; }

        public IdeaListItem()
        {

        }

        public IdeaListItem(int id, string title, string creator, string component, string status, bool displayForEmployee, bool canAccess,
            string location, DateTimeOffset created, DateTimeOffset modified) 
            :base(id, created, modified)
        {
            Title = title;
            Creator = creator;
            Component = component;
            Status = status;
            DisplayForEmployee = displayForEmployee;
            CanAccess = canAccess;
            Location = location;
        }

    }
}
