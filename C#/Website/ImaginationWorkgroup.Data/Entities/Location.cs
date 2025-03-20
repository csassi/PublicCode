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
    public class Location : ImaginationEntityBase
    {
        [Required]
        [StringLength(100)]
        public virtual string City { get; set; }
        [Required]
        public virtual bool Enabled { get; set; }
    }
}
