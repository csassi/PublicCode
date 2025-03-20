using NH.Common.AutoMapping.Attributes;
using NH.Common.AutoMapping.CustomConventions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities
{
    public class StatusMap : ImaginationEntityBase
    {
        [Required]
        public virtual IdeaStatus CurrentStatus { get; set; }
        [Required]
        public virtual MapAttribute NextMap { get; set; }
        [Required]
        public virtual int DisplayOrder { get; set; }
    }
}
