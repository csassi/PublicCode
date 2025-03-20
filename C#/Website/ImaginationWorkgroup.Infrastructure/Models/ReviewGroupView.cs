using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class ReviewGroupView : ViewModelBase
    {
        public string GroupName { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }

        public ReviewGroupView()
        {

        }

        public ReviewGroupView(string groupName, string status, int statusId, int id, DateTimeOffset created, DateTimeOffset modified)
        {
            GroupName = groupName;
            Status = status;
            StatusId = statusId;
            Id = id;
            Created = created;
            Modified = modified;
        }
    }

}
