using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public class FeatureFlag : ImaginationEntityBase
    {
        public virtual string Feature { get; set; }
        public virtual bool Active { get; set; }
    }
}
