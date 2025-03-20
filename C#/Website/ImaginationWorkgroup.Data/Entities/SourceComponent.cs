using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    [GeneratedByAssigned]
    public class SourceComponent : ImaginationEntityBase
    {
        [Required]
        [StringLength(100)]
        public virtual string Component { get; set; }
        [Required]
        public virtual bool Enabled { get; set; }
        [Required]
        public virtual short SortOrder { get; set; }
    }
}
