using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Providers;
using ImaginationWorkgroup.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public class ReviewerService : IReviewerService
    {
        private IIdeaDomain _domain;
        private IUserInfoProvider _userProvider;
        public ReviewerService(IIdeaDomain domain, IUserInfoProvider userProvider)
        {
            _domain = domain;
            _userProvider = userProvider;
        }

        public IEnumerable<ElevatedAccessLevel> CheckElevatedAccess(Idea idea, string user)
        {
            if (idea == null)
                throw new ArgumentNullException(nameof(idea));
            user = user.Split('\\').First();
            this.Log().Info($"Checking access for Idea {idea.Id} at status {idea.CurrentStatus.Status} for {user}");

            var accessLevels = new List<ElevatedAccessLevel>();
            IEnumerable<string> distros = new List<string>();
            if (idea.CurrentStatus.IsComponentRestricted)
            {
                this.Log().Info($"Checking for component membership");
                distros = _domain.GetContacts(idea.Component, idea.WorkLocation).Select(cc => cc.DistributionList.Name);
                this.Log().Info($"{distros.Count()} distributions lists found for {idea.Component.Component} - {idea.WorkLocation.City}");
            }
            else
            {
                this.Log().Info("Checking for review group membership");
                var reviewGroups = _domain.GetReviewGroups(idea.CurrentStatus.Id);
                var isDirectMember = reviewGroups
                    .SelectMany(rg => rg.Members
                        .Where(member => member.Location.Id == idea.WorkLocation.Id && member.Enabled && member.Employee.UserPin == user))
                    .Any();
                if (isDirectMember)
                    accessLevels.Add(ElevatedAccessLevel.Reviewer);

                distros = reviewGroups.
                    SelectMany(rg => rg.Distros.Where(distro => distro.Location.Id == idea.WorkLocation.Id && distro.Enabled)).Select(r => r.Name);

            }
            //if the user is a member of any of the distros associated with the review groups or the component, they are a reviewer
            foreach(var distro in distros.Distinct())
            {
                if (_userProvider.IsUserInGroup(user, distro))
                    accessLevels.Add(ElevatedAccessLevel.Reviewer);
            }

            if (idea.Employee.UserPin == user || idea.EmployeeSupervisor?.UserPin == user)
                accessLevels.Add(ElevatedAccessLevel.User);

            return accessLevels.Distinct();
        }

        public IEnumerable<ElevatedAccessLevel> CheckElevatedAccess(int ideaId, string user)
        {
            var idea = _domain.GetIdea(ideaId);
            if (idea == null)
                throw new ArgumentException($"Idea id {ideaId} could not be found", nameof(ideaId));
            return CheckElevatedAccess(idea, user);

        }

        public IEnumerable<ReviewerInfo> GetEmployeesFromReviewGroupsOrComponents(int ideaId)
        {
            var idea = _domain.GetIdeas().FirstOrDefault(i => i.Id == ideaId);
            if (idea == null)
                throw new ArgumentException($"Idea Id {ideaId} could not be found", nameof(ideaId));

            if (idea.CurrentStatus.IsComponentRestricted)
            {
                var componentContacts = _domain.GetContacts(idea.Component, idea.WorkLocation);
                if(componentContacts == null || !componentContacts.Any())
                {
                    return GetEmployeeFromReviewGroup(idea);
                }
                else
                {
                    var reviewersInComponent = new List<Data.Entities.UserInfo>();
                    var distros = componentContacts.Select(cc => cc.DistributionList.Name);
                    foreach(var distro in distros)
                    {
                        reviewersInComponent.AddRange(_userProvider.GetGroupMembers(distro).Where(u => u != Data.Entities.UserInfo.NotFound));
                    }

                    return reviewersInComponent.Select(r => new ReviewerInfo(r.First, r.Last, r.Email)).Distinct();
                }
            }
            else
            {
                return GetEmployeeFromReviewGroup(idea);
            }
        }

        private IEnumerable<ReviewerInfo> GetEmployeeFromReviewGroup(Idea idea)
        {
            var groups = _domain.GetReviewGroups(idea.CurrentStatus.Id);
            var directMembers = groups.SelectMany(rg => rg.Members.Where(member => member.Location.Id == idea.WorkLocation.Id && member.Enabled));
            var indirectMembers = new List<Data.Entities.UserInfo>();
            var distros = groups.SelectMany(rg => rg.Distros.Where(distro => distro.Location.Id == idea.WorkLocation.Id && distro.Enabled));
            foreach (var distro in distros)
            {
                indirectMembers.AddRange(_userProvider.GetGroupMembers(distro.Name).Where(u => u != Data.Entities.UserInfo.NotFound));
            }

            var allEmails = directMembers.Select(dm => new ReviewerInfo(dm.Employee.FirstName, dm.Employee.LastName, dm.Employee.Email))
                .Union(indirectMembers.Select(im => new ReviewerInfo(im.First, im.Last, im.Email)));
            return allEmails
                  .GroupBy(p => p.Email)
                  .Select(g => g.First())
                  .ToList();
        }


        public IEnumerable<ReviewGroupView> GetReviewGroupsForUser(string user)
        {
            user = user.Split('\\').Last();
            var groups = _domain.GetReviewGroups();
            var userGroups = groups.Where(g => g.Members.Any(m => m.Employee.UserPin == user && m.Enabled)).ToList();

            //need to get the distros a user is a member of from AD and compare to our list here. 
            
            var reviewGroupDistros = _domain.GetReviewGroupDistros().Where(d => d.Enabled);
            var distinctAdGroups = reviewGroupDistros.Select(rgd => rgd.Name).Distinct();

            var groupMembership = distinctAdGroups.Where(adGroup => _userProvider.IsUserInGroup(user, adGroup));

            var matchedGroups = reviewGroupDistros.Join(groupMembership, rgd => rgd.Name, gm => gm, (rgd, gm) => rgd);
            foreach (var distro in matchedGroups)
            {
                userGroups.Add(distro.ReviewGroup);
            }
            var rgStatuses = _domain.GetAllReviewGroupStatuses()
                .Join(userGroups, rgs => rgs.ReviewGroup.Id, ug => ug.Id, (rgs, ug) => new { Rgs = rgs, Grp = ug });

            return rgStatuses.Select(r => new ReviewGroupView(r.Grp.Name, r.Rgs.Status.Status, r.Rgs.Status.Id, r.Grp.Id, r.Grp.Created, r.Grp.Modified)).Distinct();
        }
    }
}
