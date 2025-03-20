using System.ComponentModel.DataAnnotations;

namespace ImaginationWorkgroup.Data.Entities.Email
{
    public class EmailAddressee : EmailerBase
    {
        public virtual EmailHeader Email { get; set; }
        [StringLength(100)]
        public virtual string Addressee { get; set; }
    }
}
