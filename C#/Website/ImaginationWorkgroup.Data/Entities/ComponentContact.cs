using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    public class ComponentContact : ImaginationEntityBase
    {
        [Required]
        public virtual SourceComponent SourceComponent { get; set; }
        [Required]
        public virtual DistributionList DistributionList { get; set; }
        [Required]
        public virtual Location Location { get; set; }
    }
}
