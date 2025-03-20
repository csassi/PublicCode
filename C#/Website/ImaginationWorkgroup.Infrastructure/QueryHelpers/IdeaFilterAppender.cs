using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.QueryHelpers
{
    public class IdeaFilterAppender : IIdeaFilterAppender
    {
        private class Groups
        {
            public static readonly int Review = 1;
            public static readonly int InProgress = 2;
            public static readonly int Implemented = 3;
            public static readonly int NotImplemented = 4;
        }

        private static IDictionary<FilterType, int[]> _filterStatusGroupMap;
        private IReviewerService _reviewerService;
        private IIdeaDomain _ideaDomain;

        static IdeaFilterAppender()
        {
            _filterStatusGroupMap = new Dictionary<FilterType, int[]>()
            {
                {FilterType.InProgress, new [] {Groups.InProgress } },
                {FilterType.InReview, new [] {Groups.Review } },
                {FilterType.Open, new [] {Groups.InProgress, Groups.Review } },
                {FilterType.Implemented, new [] {Groups.Implemented} },
                {FilterType.NotAdopted, new [] {Groups.NotImplemented} },
            };
        }

        public IdeaFilterAppender(IReviewerService reviewerService, IIdeaDomain ideaDomain)
        {
            _reviewerService = reviewerService;
            _ideaDomain = ideaDomain;
        }

        public IEnumerable<Idea> ApplyFilters(IEnumerable<Idea> baseEnumerable, FilterType filterType, string user)
        {
            baseEnumerable = ApplyStatusGroupFilters(baseEnumerable, filterType);
            if (filterType == FilterType.MyIdeas)
                baseEnumerable = ApplyUserFilter(baseEnumerable, user);
            else if(filterType == FilterType.MyWork)
            {
                baseEnumerable = ApplyWorkFilter(baseEnumerable, user);
                baseEnumerable = ApplyComponentLimitedFilter(baseEnumerable, user);
            }
            return baseEnumerable;
        }

        private IEnumerable<Idea> ApplyStatusGroupFilters(IEnumerable<Idea> baseEnumerable, FilterType filterType)
        {
            if (_filterStatusGroupMap.ContainsKey(filterType))
            {
                var groups = _filterStatusGroupMap[filterType];
                baseEnumerable = baseEnumerable.Where(i => groups.Contains(i.CurrentStatus.StatusGroup.Id));
            }
            return baseEnumerable;
        }

        private IEnumerable<Idea> ApplyUserFilter(IEnumerable<Idea> baseEnumerable, string user)
        {
            return baseEnumerable.Where(i => i.Employee.UserPin == user);
        }

        private IEnumerable<Idea> ApplyWorkFilter(IEnumerable<Idea> baseEnumerable, string user)
        {
            var myGroups = _reviewerService.GetReviewGroupsForUser(user).ToList();
            var groupStatuses = _ideaDomain.GetAllReviewGroupStatuses()
                .Where(rgs => myGroups.Select(g => g.Id).Contains(rgs.ReviewGroup.Id))
                .Where(rgs => rgs.Status.StatusGroup.Id != Groups.Implemented)
                .Select(rgs => rgs.Status.Id).ToList();

            return baseEnumerable.Where(i => groupStatuses.Contains(i.CurrentStatus.Id));

        }

        private IEnumerable<Idea> ApplyComponentLimitedFilter(IEnumerable<Idea> baseEnumerable, string user)
        {
            //make sure we hydrate a list of IDs rather than leaving as IEnumerable to improve query performance
            var componentLimitedStatuses = _ideaDomain.GetIdeaStatuses()
                .Where(iStat => iStat.IsComponentRestricted)
                .Where(iStat => iStat.StatusGroup.Id != Groups.Implemented)
                .Select(iStat => iStat.Id).ToList();    
            return baseEnumerable.Union(_ideaDomain.GetIdeas().Where(idea => componentLimitedStatuses.Contains(idea.CurrentStatus.Id)));
        }
        
    }
}
