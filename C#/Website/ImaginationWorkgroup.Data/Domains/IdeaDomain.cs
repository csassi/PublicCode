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
    public class IdeaDomain : IIdeaDomain
    { 
        IRepository _repo { get; }
        public IdeaDomain(IRepository theRepo)
        {
            _repo = theRepo;
        }

        public IRepository GetRepo()
        {
            return _repo;
        }

        public IEnumerable<Idea> GetIdeas()
        {
            return _repo.Query<Idea>();
        }
        public IEnumerable<HistoryItem> GetIdeaHistoryChanges(Idea theIdea)
        {
            var theHistories = _repo.Query<History>().Where(x => x.EntityType == "Idea" && x.EntityId == theIdea.Id);
            var theHistoryItems = _repo.Query<HistoryItem>().Where(x => theHistories.Any(y => y.Id == x.History.Id));
            return theHistoryItems;
        }

        public StatusMap GetMapForIdea(Idea idea)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));
            var map = _repo.Query<StatusMap>().Where(sm => sm.CurrentStatus == idea.CurrentStatus).FirstOrDefault();
            if (map == null)
                throw new IndexOutOfRangeException($"There is no status in the statemap that matches the status [{idea.CurrentStatus.Id}] of idea {idea.Id}");
            return map;
        }

        public IEnumerable<ReviewGroup> GetReviewGroups()
        {
            var groups = _repo.Query<ReviewGroup>();
            return groups;
        }

        public IEnumerable<ReviewGroupDistro> GetReviewGroupDistros()
        {
            return _repo.Query<ReviewGroupDistro>();
        }

        public IEnumerable<ReviewGroup> GetReviewGroups(int statusId)
        {
            var groups = _repo.Query<ReviewGroupStatus>()
                .Where(rgs => rgs.Status.Id == statusId && rgs.Enabled)
                .Select(rgs => rgs.ReviewGroup);
            return groups;
        }

        public IEnumerable<ReviewGroupStatus> GetAllReviewGroupStatuses()
        {
            return _repo.Query<ReviewGroupStatus>();
        }

        public IEnumerable<ComponentContact> GetContacts(SourceComponent sourceComponent, Location location)
        {
            return _repo.Query<ComponentContact>().Where(cc => cc.SourceComponent == sourceComponent && cc.Location == location);
        }

        public IEnumerable<StatusMap> GetMapsForIdea(Idea idea)
        {
            if (idea == null)
            {
                throw new ArgumentNullException(nameof(idea));
            }
            return _repo.Query<StatusMap>().Where(sm => sm.CurrentStatus.Id == idea.CurrentStatus.Id);
        }

        public StatusMap GetMapById(int statusMapId)
        {
            return _repo.Get<StatusMap>(statusMapId);
        }

        public IEnumerable<IdeaStatus> GetIdeaStatuses()
        {
            return _repo.Query<IdeaStatus>();
        }

        public Idea GetIdea(int id)
        {
            return _repo.Get<Idea>(id);
        }
    }
}
