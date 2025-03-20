using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    [GeneratedByAssigned]
    public class IdeaDescriptionType : ImaginationEntityBase
    {
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }
        [Required]
        [StringLength(500)]
        public virtual string Placeholder { get; set; }
        [Required]
        public virtual int Length { get; set; }
        [Required]
        public virtual bool Required { get; set; }
        [Required]
        public virtual short SortOrder { get; set; }
    }
}
