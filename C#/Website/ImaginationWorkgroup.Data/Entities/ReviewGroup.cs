using NH.Common.AutoMapping.CustomConventions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    [GeneratedByAssigned]
    public class ReviewGroup : ImaginationEntityBase
    {
        [Obsolete("This property will be removed")]
        public virtual IdeaStatus ReviewStatus { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual IList<ReviewGroupMember> Members { get; set; }
        public virtual IList<ReviewGroupDistro> Distros { get; set; }

        public ReviewGroup()
        {
            Members = new List<ReviewGroupMember>();
        }
    }
}
