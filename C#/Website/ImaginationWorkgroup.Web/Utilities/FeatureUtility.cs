using ImaginationWorkgroup.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImaginationWorkgroup.Web.Utilities
{
    public class FeatureUtility
    {
        public static IFeatureFlagProvider GetFeatures()
        {
            var type = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IFeatureFlagProvider));
            return System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IFeatureFlagProvider)) as IFeatureFlagProvider;
        }
    }
}