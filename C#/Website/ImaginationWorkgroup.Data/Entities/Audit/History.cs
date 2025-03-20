using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities.Audit
{
    public class History : ImaginationEntityBase, INoAuditTrigger
    {
        public virtual string EntityType { get; set; }
        public virtual int EntityId { get; set; }
        public virtual Idea AssociatedIdea { get; set; }
        public virtual EmployeeProfile ChangingEmployee { get; set; }
        public History()
        {
            
        }

        public History(string entityType, int entityId, Idea associatedIdea, EmployeeProfile changingEmployee)
        {
            EntityType = entityType;
            EntityId = entityId;
            AssociatedIdea = associatedIdea;
            ChangingEmployee = changingEmployee;
        }
    }
}
