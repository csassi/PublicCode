using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class IdeaDetailsStatusChange
    {
        public String FromStatus { get; set; }
        public String ToStatus { get; set; }
        public String Who { get; set; }
        public String When { get; set; }
    }
    public class IdeaDetailsModel
    {
        public IdeaDetailsModel()
        {
        }
        public int Id { get; set; }//Idea id
        public String Idea { get; set; }
        public String Problem { get; set; }
        public String Benefits { get; set; }
        public String Title { get; set; }
        public String Component { get; set; }
        public String Supervisor { get; set; }
        public String Employee { get; set; }
        public String Status { get; set; }
        public String ModifiedBy { get; set; }
        public String Created { get; set; }
        public String Modified { get; set; }
        public String Location { get; set; }
        public List<StatusMapView> StatusMaps { get; set; }
        //Possibly being used in email.
        public List<IdeaComment> Comments { get;set; }
        public List<IdeaDetailsStatusChange> StatusChanges { get; set; }
        public Boolean RenderCommentButton { get; set; }
        public Boolean CanReviewIdea { get; set; }
        public LocationChangeModel LocationChangeModel { get; set; }
        public String WorkLocation { get; set; }
    }
}
