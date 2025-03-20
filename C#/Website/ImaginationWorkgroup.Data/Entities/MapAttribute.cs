using NH.Common.AutoMapping.CustomConventions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities
{
    [GeneratedByAssigned]
    public class MapAttribute : ImaginationEntityBase
    {
        [Required]
        public virtual IdeaStatus Status { get; set; }
        [Required]
        public virtual string DisplayText { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string ButtonClass { get; set; }
        [Required]
        [StringLength(1000)]
        public virtual string PromptText { get; set; }
        [Required]
        public virtual bool RequireComments { get; set; }
        [Required]
        [StringLength(500)]
        public virtual string PromptPlaceholder { get; set; }

    }
}
