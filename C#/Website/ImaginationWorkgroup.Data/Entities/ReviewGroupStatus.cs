using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public class ReviewGroupStatus : ImaginationEntityBase
    {
        public virtual ReviewGroup ReviewGroup { get; set; }
        public virtual IdeaStatus Status { get; set; }
        public virtual bool Enabled { get; set; }

    }
}
