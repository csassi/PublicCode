using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities.Email
{
    public class EmailAttachment : EmailerBase
    {
        public virtual EmailHeader Email { get; set; }
        [StringLength(250)]
        public virtual string ContentType { get; set; }
        public virtual byte[] Contents { get; set; }
        [StringLength(50)]
        public virtual string Filename { get; set; }
    }
}
