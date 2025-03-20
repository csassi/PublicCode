using ImaginationWorkgroup.Data.Dto;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Audit;
using ImaginationWorkgroup.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public interface IIdeaService
    {
        EmployeeProfile CreateProfile(EmployeeProfile theProfile);
        EmployeeProfile CheckProfile(EmployeeProfile theProfileToCheck);
        EmployeeProfile GetMostRecentProfile(String thePin);
        Idea CreateIdea(Idea theIdea);
        Idea UpdateIdea(Idea theIdea);
        IdeaDescription CreateDescription(IdeaDescription theIdeaDescription);
        List<IdeaDescriptionType> GetAllDescriptionTypes();
        SourceComponent GetComponent(String theComponent);
        SourceComponent GetComponent(int theComponentId);
        IEnumerable<SourceComponent> GetComponents();
        IEnumerable<Location> GetLocations();
        Location GetLocation(int theLocationId);
        IEnumerable<HistoryItem> GetIdeaStatusChanges(Idea theIdea);
        IEnumerable<IdeaListItem> GetIdeas(FilterType filterType, string userPin, bool isReviewer);
        IEnumerable<IdeaStatus> GetIdeaStatuses();
        IdeaStatus GetIdeaStatus(int id);
        Idea GetIdea(int theId);
        IdeaDetailsModel ConvertIdeaToDetailsModel(Idea theIdeaToConvert);
        IdeaComment CreateComment(IdeaComment theComment);
        IEnumerable<IdeaComment> GetIdeaComments(int theIdeaId);
        //TODO: EmployeeProfile for modified by is passed in but nothing happens. This will be added with the history changes. 

        [Obsolete("This method will need to change to provide a target MapAttributeId rather than a route update type.")]
        void RouteIdea(Idea idea, EmployeeProfile modifiedBy, RouteUpdateType updateType);
        void RouteIdea(Idea idea, EmployeeProfile modifiedBy, int selectedStatusMap, string comment);

        void UpdateIdeaWorkLocation(int ideaId, int newLocationId);
    }
}
