using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    public class DistributionList : ImaginationEntityBase
    {
        [Required]
        [StringLength(500)]
        public virtual string Name { get; set; }

    }
}
