using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using ImaginationWorkgroup.Data.Entities.Email;

namespace ImaginationWorkgroup.Data.MappingOverrides
{
    public class EmailAddresseeOverride : IAutoMappingOverride<EmailAddressee>
    {
        public void Override(AutoMapping<EmailAddressee> mapping)
        {
            mapping.Id(m => m.Id, "addressee_id").GeneratedBy.Identity();
        }
    }
}
