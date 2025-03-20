using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public class UsageService : IUsageService
    {
        private IFeatureDomain _domain;
        public UsageService(IFeatureDomain domain)
        {
            _domain = domain;
        }
        public void RecordUsage(UsageData data, string user)
        {
            var featureUtiliziation = new FeatureUtilization(data.Pathname, data.Query, data.AdditionalData, user);
            _domain.RecordUtilization(featureUtiliziation);
        }
    }
}
