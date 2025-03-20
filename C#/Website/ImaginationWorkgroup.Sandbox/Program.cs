using ImaginationWorkgroup.Data.Configuration;
using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Providers;
using ImaginationWorkgroup.Data.Repositories;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.RegistrationByConvention;

namespace ImaginationWorkgroup.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = @"Data Source=someDB\dev; Initial Catalog=blarg; Integrated Security=true;";
            NhConfigurator.GenerateDbScript(conn);


            var container = new UnityContainer();

            container.RegisterTypes(
            AllClasses.FromAssemblies(Assembly.GetAssembly(typeof(ViewModelBase)), 
                    Assembly.GetAssembly(typeof(ImaginationEntityBase)), 
                    Assembly.GetAssembly(typeof(IdeaDomain))),
            WithMappings.FromMatchingInterface,
            WithName.Default).
            RegisterType<IUserInfoProvider, ADUserInfoProvider>();


            var ideaDomain = container.Resolve<IIdeaDomain>();
            var reviewerService = container.Resolve<IReviewerService>();
            var myGroups = reviewerService.GetReviewGroupsForUser("1234567");
            var groupStatuses = ideaDomain.GetAllReviewGroupStatuses()
                .Where(rgs => myGroups.Select(g => g.Id).Contains(rgs.ReviewGroup.Id))
                .Where(rgs => rgs.Status.StatusGroup.Id != 3)
                .Select(rgs => rgs.Status.Id).ToList();

            var results = ideaDomain.GetIdeas().Where(i => groupStatuses.Contains(i.CurrentStatus.Id)).ToList();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

    }
}
