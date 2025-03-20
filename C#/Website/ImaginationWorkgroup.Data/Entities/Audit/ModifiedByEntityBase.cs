using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities.Audit
{
    public abstract class ModifiedByEntityBase : ImaginationEntityBase
    {
        public virtual EmployeeProfile ModifiedBy { get; set; }
    }
}
