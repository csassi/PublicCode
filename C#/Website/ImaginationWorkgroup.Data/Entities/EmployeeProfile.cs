using System.ComponentModel.DataAnnotations;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    public class EmployeeProfile : ImaginationEntityBase
    {
        [Required]
        [StringLength(15)]
        public virtual string UserPin { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string LastName { get; set; }
        [Required]
        [StringLength(15)]
        public virtual string Office { get; set; }
        [Required]
        [StringLength(15)]
        public virtual string Position { get; set; }
        [Required]
        [StringLength(15)]
        public virtual string Mod { get; set; }
        [Required]
        public virtual string Email { get; set; }
    }
}
