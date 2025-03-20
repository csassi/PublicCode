using ImaginationWorkgroup.Infrastructure.Models.Api;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public interface IUsageService
    {
        void RecordUsage(UsageData data, string user);
    }
}
