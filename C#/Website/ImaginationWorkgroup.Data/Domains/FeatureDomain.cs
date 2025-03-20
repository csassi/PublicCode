using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Repositories;

namespace ImaginationWorkgroup.Data.Domains
{
    public class FeatureDomain : IFeatureDomain
    {
        private IRepository _repo;
        public FeatureDomain(IRepository repo)
        {
            _repo = repo;
        }
        public void RecordUtilization(FeatureUtilization utilization)
        {
            _repo.Add(utilization);
        }

        //use this class later if we start to manage feature flags through the website. 
    }
}
