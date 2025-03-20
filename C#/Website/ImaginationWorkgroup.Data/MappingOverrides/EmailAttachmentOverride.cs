using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using ImaginationWorkgroup.Data.Entities.Email;

namespace ImaginationWorkgroup.Data.MappingOverrides
{
    public class EmailAttachmentOverride : IAutoMappingOverride<EmailAttachment>
    {
        public void Override(AutoMapping<EmailAttachment> mapping)
        {
            mapping.Id(m => m.Id, "attachment_id").GeneratedBy.Identity();
        }
    }
}
