using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public abstract class ImaginationEntityBase
    {
        public virtual int Id { get; set; }
        [Required]
        public virtual DateTimeOffset Created { get; set; }
        [Required]
        public virtual DateTimeOffset Modified { get; set; }

        public ImaginationEntityBase()
        {
            Created = DateTimeOffset.Now;
            Modified = DateTimeOffset.Now;
        }
    }
}
