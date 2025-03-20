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
    public class StatusGroup : ImaginationEntityBase
    {
        [StringLength(100)]
        public virtual string Group { get; set; }
    }
}
