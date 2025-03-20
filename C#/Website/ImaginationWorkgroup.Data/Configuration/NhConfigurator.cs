using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Email;
using ImaginationWorkgroup.Data.MappingOverrides;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cc = NH.Common.AutoMapping.CustomConventions;

namespace ImaginationWorkgroup.Data.Configuration
{
    public class NhConfigurator
    {
        public static ISessionFactory GetSessionFactory(string connectionString)
        {
            var cfg = GetNHibernateConfiguration(connectionString);
            return cfg.BuildSessionFactory();
        }
        public static NHibernate.Cfg.Configuration GetNHibernateConfiguration(string connectionString)
        {
            var cfg = Fluently.Configure()
               .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
               .Mappings(map => map.AutoMappings
                   .Add(
                       AutoMap.AssemblyOf<NhConfigurator>().Where(type => type.IsSubclassOf(typeof(ImaginationEntityBase)))
                       .Conventions.Add<cc.PrimaryKey.SetGeneratedByAssignedForAttribute>()
                       .Conventions.Add<cc.TableName.PluralizedPascalCase>()
                       .Conventions.Add<cc.ColumnName.PropertyNameExcludeEnum>()
                       .Conventions.Add<cc.ForeignKey.PascalCaseAppendId>()
                       .Conventions.Add<cc.Enum.PascalCaseAppendId>()
                       .Conventions.Add<cc.PrimaryKey.SetGeneratedByAssignedForAttribute>()
                       .Conventions.Add<cc.Attributes.StringLength>()
                       .Conventions.Add<cc.Attributes.Required>()
                       .Conventions.Add<cc.Attributes.CreateIndex>()
                   )
                   .Add(
                        AutoMap.AssemblyOf<NhConfigurator>().Where(type => type.IsSubclassOf(typeof(EmailerBase)) && !type.IsAbstract)
                        .Conventions.Add<cc.TableName.PluralizedLowerUnderscore>()
                        .Conventions.Add<cc.ColumnName.LowerUnderscorePropertyNameExcludeEnum>()
                       .Conventions.Add<cc.ForeignKey.LowerPropertyUnderscoreId>()
                       .Conventions.Add<cc.Enum.LowerPropertyUnderscoreId>()
                       .Conventions.Add<cc.PrimaryKey.ClassNameLowerUnderscoreId>()
                       .Conventions.Add<cc.Attributes.StringLength>()
                       .UseOverridesFromAssemblyOf<EmailAddresseeOverride>()
                   )
               )
               
               .ExposeConfiguration(c =>
               {
                   SchemaMetadataUpdater.QuoteTableAndColumns(c, Dialect.GetDialect(c.GetDerivedProperties()));
                   c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new NhEventListener() };
                   c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new NhEventListener() };
                   c.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] { new NhEventListener() };
               })
               .BuildConfiguration();
            return cfg;
        }

        public static void GenerateDbScript(string connectionString)
        {
            var config = GetNHibernateConfiguration(connectionString);
            var update = new NHibernate.Tool.hbm2ddl.SchemaUpdate(config);
            update.Execute(true, false);
            Console.WriteLine("--end of schema generation");
        }
    }
}
