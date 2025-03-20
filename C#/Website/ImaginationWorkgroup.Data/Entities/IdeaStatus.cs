using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    [GeneratedByAssigned]
    public class IdeaStatus : ImaginationEntityBase
    {
        [Required]
        [StringLength(100)]
        public virtual string Status { get; set; }
        [Required]
        public virtual bool Enabled { get; set; }
        [Required]
        public virtual short SortOrder { get; set; }
        [Required]
        public virtual StatusGroup StatusGroup { get; set; }
        [Required]
        public virtual bool DisplayForEmployee { get; set; }
        [Required]
        public virtual bool IsComponentRestricted { get; set; }
        [Required]
        public virtual bool CanChangeLocation { get; set; }
    }
}
