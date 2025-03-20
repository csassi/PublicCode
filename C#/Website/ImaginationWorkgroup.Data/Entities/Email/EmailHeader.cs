using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities.Email
{
    public class EmailHeader : EmailerBase
    {
        [StringLength(50)]
        public virtual string EmailFrom { get; set; }
        [StringLength(100)]
        public virtual string Subject { get; set; }
        [StringLength(int.MaxValue)]
        public virtual string Body { get; set; }
        public virtual short Priority { get; set; }
        public virtual bool Sent { get; set; }
        [StringLength(50)]
        public virtual string System { get; set; }
        public virtual bool IsBodyHtml { get; set; }
    }
}
