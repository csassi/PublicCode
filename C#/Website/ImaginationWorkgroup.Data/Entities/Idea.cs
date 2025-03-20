using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationWorkgroup.Data.Entities.Audit;
using NH.Common.AutoMapping.Attributes;
using NH.Common.AutoMapping.CustomConventions.Attributes;

namespace ImaginationWorkgroup.Data.Entities
{
    public class Idea : ModifiedByEntityBase
    {
        [StringLength(100)]
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual EmployeeProfile Employee { get; set; }
        [Required]
        public virtual EmployeeProfile EmployeeSupervisor { get; set; }
        [Required]
        public virtual SourceComponent Component { get; set; }
        [Required]
        public virtual IdeaStatus CurrentStatus { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location WorkLocation { get; set; }
        [LazyLoad]
        public virtual IEnumerable<IdeaDescription> Descriptions { get; set; }
        [LazyLoad]
        public virtual IEnumerable<IdeaComment> Comments { get; set; }

    }
}
