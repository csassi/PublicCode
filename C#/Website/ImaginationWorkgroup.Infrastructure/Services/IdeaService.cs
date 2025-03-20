using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Dto;
using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Audit;
using ImaginationWorkgroup.Data.Repositories;
using ImaginationWorkgroup.Infrastructure.Exceptions;
using ImaginationWorkgroup.Infrastructure.Models;
using ImaginationWorkgroup.Infrastructure.QueryHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Services
{
    public class IdeaService : IIdeaService
    {
        IIdeaDomain _theDomain;
        IIdeaFilterAppender _filterAppender;
        private IReviewerService _reviewerService;

        public IdeaService(IIdeaDomain theDomain, IIdeaFilterAppender filterAppender, IReviewerService reviewerService)
        {
            _theDomain = theDomain;
            _filterAppender = filterAppender;
            _reviewerService = reviewerService;
        }

        //public IEnumerable<ReviewGroupMember> GetReviewers(IdeaStatus theStatus, Location theLocation)
        //{
        //    var theReviewGroup = _theDomain.GetRepo().Query<ReviewGroup>().Where(x => x.ReviewStatus.Id == theStatus.Id).FirstOrDefault();
        //    if(theReviewGroup == null)
        //    {
        //        return null;
        //    }
        //    return _theDomain.GetRepo().Query<ReviewGroupMember>().Where(x => x.ReviewGroup.Id == theReviewGroup.Id && x.Location.Id == theLocation.Id);
        //}
        //public IEnumerable<ReviewGroupMember> GetReviewers()
        //{
        //    return _theDomain.GetRepo().Query<ReviewGroupMember>();
        //}
        public IEnumerable<IdeaStatus> GetIdeaStatuses()
        {
            return _theDomain.GetRepo().Query<IdeaStatus>();
        }
        public IdeaStatus GetIdeaStatus(int id)
        {
            return _theDomain.GetRepo().Get<IdeaStatus>(id);
        }
        public Idea GetIdea(int theId)
        {
            return _theDomain.GetRepo().Get<Idea>(theId);
        }
        public Idea CreateIdea(Idea theIdea)
        {
            _theDomain.GetRepo().Add(theIdea);
            return theIdea;
        }
        public Idea UpdateIdea(Idea theIdea)
        {
            _theDomain.GetRepo().Update(theIdea);
            return theIdea;
        }
        public IEnumerable<IdeaComment> GetIdeaComments(int theIdeaId)
        {
            return _theDomain.GetRepo().Query<IdeaComment>().Where(x => x.Idea.Id == theIdeaId);
        }
        public IEnumerable<HistoryItem> GetIdeaStatusChanges(Idea theIdea)
        {
            var theItems = _theDomain.GetIdeaHistoryChanges(theIdea).Where(x => x.Property == "CurrentStatus");
            return theItems.OrderByDescending(x => x.Modified);
        }
        public List<IdeaDetailsStatusChange> ConvertHistoryStatusChangesToStatusModel(IEnumerable<HistoryItem> theStatusChanges)
        {
            var theList = new List<IdeaDetailsStatusChange>();
            foreach (var item in theStatusChanges)
            {
                var theStatusFrom = GetIdeaStatus(Int32.Parse(item.OldValue));
                var theStatusTo = GetIdeaStatus(Int32.Parse(item.NewValue));

                var theStatusModel = new IdeaDetailsStatusChange()
                {
                    FromStatus = theStatusFrom.Status,
                    ToStatus = theStatusTo.Status,
                    When = item.Modified.DateTime.ToString("MM/dd/yyyy hh:mm tt"),
                    Who = $"{item.History.ChangingEmployee.FirstName} {item.History.ChangingEmployee.LastName}"
                };
                theList.Add(theStatusModel);
            }

            return theList;
        }

        public IdeaComment CreateComment(IdeaComment theComment)
        {
            _theDomain.GetRepo().Add(theComment);
            return theComment;
        }
        public IdeaDetailsModel ConvertIdeaToDetailsModel(Idea theIdea)
        {
            var statusMaps = _theDomain.GetMapsForIdea(theIdea);
            var theStatusChanges = GetIdeaStatusChanges(theIdea);
            IdeaDetailsModel theModel = new IdeaDetailsModel
            {
                Idea = theIdea.Descriptions.Where(x => x.DescriptionType.Name == "Idea").FirstOrDefault().DescriptionValue,
                Problem = theIdea.Descriptions.Where(x => x.DescriptionType.Name == "Problem").FirstOrDefault().DescriptionValue,
                Benefits = theIdea.Descriptions.Where(x => x.DescriptionType.Name == "Benefits").FirstOrDefault().DescriptionValue,
                Component = theIdea.Component.Component,
                Created = theIdea.Created.DateTime.ToString("MM/dd/yyyy hh:mm tt"),
                Modified = theIdea.Modified.DateTime.ToString("MM/dd/yyyy hh:mm tt"),
                Supervisor = $"{theIdea.EmployeeSupervisor.FirstName} {theIdea.EmployeeSupervisor.LastName}",
                Employee = $"{theIdea.Employee.FirstName} {theIdea.Employee.LastName}",
                Title = theIdea.Title,
                Id = theIdea.Id,
                Status = theIdea.CurrentStatus.Status,
                StatusMaps = statusMaps.Select(sm => new StatusMapView(sm)).OrderBy(sm => sm.DisplayOrder).ToList(),
                StatusChanges = ConvertHistoryStatusChangesToStatusModel(theStatusChanges),
                RenderCommentButton = true,
                ModifiedBy = $"{theIdea.ModifiedBy.FirstName} {theIdea.ModifiedBy.LastName}",
                Location = theIdea.Location.City,
                WorkLocation = theIdea.WorkLocation.City
            };

            var locationChange = new LocationChangeModel();
            locationChange.CanChangeLocation = theIdea.CurrentStatus.CanChangeLocation;
            locationChange.CurrentLocation = new LocationView(theIdea.WorkLocation.Id, theIdea.WorkLocation.City);
            var otherLocations = _theDomain.GetRepo().Query<Location>().Where(l => l.Id != theIdea.WorkLocation.Id).Where(l => l.Enabled);
            locationChange.PossibleLocations = otherLocations.AsEnumerable().Select(ol => new LocationView(ol.Id, ol.City)).OrderBy(lv => lv.Location).ToList();
            theModel.LocationChangeModel = locationChange;
            //TODO: Also check roles at some point.

            return theModel;
        }
        public IdeaDescription CreateDescription(IdeaDescription theIdeaDescription)
        {
            _theDomain.GetRepo().Add(theIdeaDescription);
            return theIdeaDescription;
        }
        public EmployeeProfile CreateProfile(EmployeeProfile theProfile)
        {
            _theDomain.GetRepo().Add(theProfile);
            return theProfile;
        }

        public EmployeeProfile GetMostRecentProfile(String thePin)
        {
            var theResults = _theDomain.GetRepo().Query<EmployeeProfile>().Where(x => x.UserPin == thePin);
            var theSortedResults = theResults.OrderByDescending(x => x.Modified);
            return theSortedResults.FirstOrDefault();
        }

        public EmployeeProfile CheckProfile(EmployeeProfile theProfileToCheck)
        {
            var theProfile = GetMostRecentProfile(theProfileToCheck.UserPin);
            if(theProfile == null)
            {
                return null;
            }

            if (theProfile.FirstName == theProfileToCheck.FirstName &&
           theProfile.LastName == theProfileToCheck.LastName && theProfile.Mod == theProfileToCheck.Mod &&
           theProfile.Office == theProfileToCheck.Office && theProfile.Position == theProfileToCheck.Position &&
           theProfile.Email == theProfileToCheck.Email && theProfile.UserPin == theProfileToCheck.UserPin)
            {
                return theProfile;
            }
            else
            {
                return theProfileToCheck;
            }
        }

        public List<IdeaDescriptionType> GetAllDescriptionTypes()
        {
            return _theDomain.GetRepo().Query<IdeaDescriptionType>().ToList();
        }
        public SourceComponent GetComponent(string theComponent)
        {
            return _theDomain.GetRepo().Query<SourceComponent>().Where(x => x.Component == theComponent).FirstOrDefault();

        }
        public SourceComponent GetComponent(int theComponentId)
        {
            return _theDomain.GetRepo().Get<SourceComponent>(theComponentId);
        }
        public IEnumerable<SourceComponent> GetComponents()
        {
            return _theDomain.GetRepo().Query<SourceComponent>().Where(x => x.Enabled == true);
        }
        public IEnumerable<Location> GetLocations()
        {
            return _theDomain.GetRepo().Query<Location>().Where(x => x.Enabled == true);
        }
        public Location GetLocation(int theLocationId)
        {
            return _theDomain.GetRepo().Get<Location>(theLocationId);
        }

        public IEnumerable<IdeaListItem> GetIdeas(FilterType filterType, string userPin, bool isReviewer)
        {
            var ideas = _theDomain.GetIdeas();

            var filteredIdeas = _filterAppender.ApplyFilters(ideas, filterType, userPin);

            var listItems = new List<IdeaListItem>();
            foreach(var idea in filteredIdeas)
            {
                var display = idea.CurrentStatus.DisplayForEmployee;
                var canAccess = true;
                
                if (!display)
                {

                    var accessType = _reviewerService.CheckElevatedAccess(idea, userPin);
                    //for the special case of hiding from employees, we also need to double check the reviewer + in role thing
                    if ((accessType.Contains(ElevatedAccessLevel.Reviewer) && !isReviewer) || !accessType.Any())
                        canAccess = false;
                    //but if they are the user, display regardless of the above scenario. 
                    if (accessType.Contains(ElevatedAccessLevel.User))
                        canAccess = true;
                }

                //if the filter type is "My Work", we need to drop anythign they are not the reviewer for
                if (filterType == FilterType.MyWork && !_reviewerService.CheckElevatedAccess(idea, userPin).Contains(ElevatedAccessLevel.Reviewer))
                {
                    this.Log().Debug($"Not including idea {idea.Id} in the my work results because {userPin} is not a reviewer");
                }
                else
                {
                    if (listItems.FirstOrDefault(li => li.Id == idea.Id) == null)
                    {
                        listItems.Add(new IdeaListItem(idea.Id, idea.Title, EmployeeDisplay(idea.Employee),
                            idea.Component?.Component, idea.CurrentStatus.Status, display, canAccess, idea.WorkLocation.City,
                            idea.Created, idea.Modified));
                    }
                    else
                    {
                        this.Log().Debug($"Idea id {idea.Id} was a duplicate in the list and is not being included twice");
                    }
                }

            }
            return listItems;
        }

        private bool IsUserOnIdea(Idea idea, string userPin)
        {
            return idea.Employee.UserPin == userPin || idea.EmployeeSupervisor.UserPin == userPin;
        }

        private string EmployeeDisplay(EmployeeProfile employee)
        {
            return $"{employee.LastName}, {employee.FirstName}";
        }

        public void RouteIdea(Idea idea, EmployeeProfile modifiedBy, RouteUpdateType updateType)
        {
            var map = _theDomain.GetMapForIdea(idea);

            //if (updateType == RouteUpdateType.Approve && map.NextMap != null)
            //{
            //    SaveIdeaStatus(idea, map.NextMap.Status);
            //}
            //else if (updateType == RouteUpdateType.Reject && map.RejectMap != null)
            //{
            //    SaveIdeaStatus(idea, map.RejectMap.Status);
            //}
            //else
            //{
            //    throw new ArgumentException($"There is no mapping for the current status {idea.CurrentStatus.Status} and route type provided", nameof(updateType));
            //}
            throw new NotImplementedException("Still need to work on the routing implementation");
        }

        public void RouteIdea(Idea idea, EmployeeProfile modifiedBy, int selectedStatusMap, string comment)
        {
            var selectedMap = _theDomain.GetMapById(selectedStatusMap);
            if (selectedMap == null)
                throw new ArgumentException("No map found for the status map id provided", nameof(selectedStatusMap));

            //check for other changes. 
            if(idea.CurrentStatus.Id != selectedMap.CurrentStatus.Id)
            {
                var message = $"The Idea {idea.Id} has been modified since the client last refreshed the browser. Current State is {idea.CurrentStatus.Status} - Expected {selectedMap.CurrentStatus.Status}";
                this.Log().Error(message);
                throw new UnexpectedStateException(message);
            }
            //save the comment if there is one
            if (!string.IsNullOrEmpty(comment))
            {
                var newMessage = $"[{idea.CurrentStatus.Status} changed to {selectedMap.NextMap.Status.Status}] with comment: {comment}";
                var newComment = new IdeaComment(newMessage, idea, modifiedBy);
                this.CreateComment(newComment);
            }
            this.SaveIdeaStatus(idea, selectedMap.NextMap.Status);
        }

        private void SaveIdeaStatus(Idea idea, IdeaStatus status)
        {
            idea.CurrentStatus = status;
            _theDomain.GetRepo().Update(idea);
        }

        public void UpdateIdeaWorkLocation(int ideaId, int newLocationId)
        {
            var idea = this.GetIdea(ideaId);
            var newLocation = this.GetLocation(newLocationId);
            idea.WorkLocation = newLocation;
            this.UpdateIdea(idea);
        }
    }
}
