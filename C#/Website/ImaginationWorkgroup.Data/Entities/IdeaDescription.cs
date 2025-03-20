using System.ComponentModel.DataAnnotations;
using ImaginationWorkgroup.Data.Entities.Audit;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    public class IdeaDescription : ModifiedByEntityBase
    {
        [Required]
        public virtual Idea Idea { get; set; }
        [Required]
        public virtual IdeaDescriptionType DescriptionType { get; set; }
        [StringLength(2000)]
        public virtual string DescriptionValue { get; set; }
    }
}
