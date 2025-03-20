using ImaginationWorkgroup.Data.Dto;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Audit;
using ImaginationWorkgroup.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Domains
{
    public interface IIdeaDomain
    {
        IRepository GetRepo();
        Idea GetIdea(int id);
        IEnumerable<Idea> GetIdeas();
        IEnumerable<HistoryItem> GetIdeaHistoryChanges(Idea theIdea);
        [Obsolete("Replacing with IEnumerable<StatusMap> after rebuilding the status maps")]
        StatusMap GetMapForIdea(Idea idea);
        IEnumerable<StatusMap> GetMapsForIdea(Idea idea);
        IEnumerable<ReviewGroup> GetReviewGroups();
        IEnumerable<ReviewGroup> GetReviewGroups(int statusId);
        IEnumerable<ReviewGroupDistro> GetReviewGroupDistros();
        IEnumerable<ReviewGroupStatus> GetAllReviewGroupStatuses();
        IEnumerable<ComponentContact> GetContacts(SourceComponent sourceComponent, Location location);
        StatusMap GetMapById(int statusMapId);
        IEnumerable<IdeaStatus> GetIdeaStatuses();
    }
}
