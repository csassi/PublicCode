using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using ImaginationWorkgroup.Data.Entities.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.MappingOverrides
{
    public class EmailHeaderOverride : IAutoMappingOverride<EmailHeader>
    {
        public void Override(AutoMapping<EmailHeader> mapping)
        {
            mapping.Id(m => m.Id, "email_id").GeneratedBy.Identity();
        }
    }
}
