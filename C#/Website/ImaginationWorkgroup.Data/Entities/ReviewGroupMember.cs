using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities
{
    public class ReviewGroupMember : ImaginationEntityBase
    {
        public virtual EmployeeProfile Employee { get; set; }
        public virtual ReviewGroup ReviewGroup { get; set; }
        public virtual Location Location { get; set; }
        [Required]
        public virtual bool Enabled { get; set; }
    }
}
