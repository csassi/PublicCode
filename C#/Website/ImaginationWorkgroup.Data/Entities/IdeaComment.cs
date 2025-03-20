using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public class IdeaComment : ImaginationEntityBase
    {
        [StringLength(2500)]
        [Required]
        public virtual string Comment { get; set; }
        public virtual Idea Idea { get; set; }
        public virtual EmployeeProfile Employee { get; set; }

        public IdeaComment() : base()
        {
            
        }

        public IdeaComment(string comment, Idea idea, EmployeeProfile employee) : this()
        {
            Comment = comment;
            Idea = idea;
            Employee = employee;
        }
    }
}
