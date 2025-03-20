using ImaginationWorkgroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Domains
{
    public interface IFeatureDomain
    {
        void RecordUtilization(FeatureUtilization utilization);
    }
}
