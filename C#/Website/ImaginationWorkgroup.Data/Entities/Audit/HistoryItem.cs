using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities.Audit
{
    public class HistoryItem : ImaginationEntityBase, INoAuditTrigger
    {
        public virtual History History { get; set; }
        public virtual string Property { get; set; }
        [StringLength(3000)]
        public virtual string OldValue { get; set; }
        [StringLength(3000)]
        public virtual string NewValue { get; set; }
        public HistoryItem()
        {

        }

        public HistoryItem(History history, string property, string oldValue, string newValue)
        {
            History = history;
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
