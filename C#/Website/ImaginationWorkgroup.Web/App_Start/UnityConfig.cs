using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Providers;
using ImaginationWorkgroup.Infrastructure.Models;
using System;
using System.Reflection;
using Unity;
using Unity.RegistrationByConvention;

namespace ImaginationWorkgroup.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromAssemblies(Assembly.GetAssembly(typeof(ViewModelBase)), Assembly.GetAssembly(typeof(ImaginationEntityBase)), Assembly.GetAssembly(typeof(IdeaDomain))),
                WithMappings.FromMatchingInterface,
                WithName.Default).
                RegisterType<IUserInfoProvider, ADUserInfoProvider>();
        }
    }
}