using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities
{
    public class ReviewGroupDistro : ImaginationEntityBase
    {
        [Required]
        public virtual ReviewGroup ReviewGroup { get; set; }
        [Required]
        public virtual Location Location { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual bool Enabled { get; set; }
    }
}
