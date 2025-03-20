using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models;
using System.Collections.Generic;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public interface IReviewerService
    {
        IEnumerable<ReviewGroupView> GetReviewGroupsForUser(string user);
        IEnumerable<ReviewerInfo> GetEmployeesFromReviewGroupsOrComponents(int ideaId);
        IEnumerable<ElevatedAccessLevel> CheckElevatedAccess(int ideaId, string user);
        IEnumerable<ElevatedAccessLevel> CheckElevatedAccess(Idea idea, string user);
    }
}
