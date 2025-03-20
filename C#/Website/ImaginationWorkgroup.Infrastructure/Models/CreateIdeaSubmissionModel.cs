using ImaginationWorkgroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class CreateIdeaSubmissionModel
    {
        public String IdeaTitle { get; set; }
        public String IdeaProblem { get; set; }
        public String IdeaText { get; set; }
        public String IdeaBenefits { get; set; }
        public String IdeaTitlePlaceholder { get; set; }
        public String IdeaProblemPlaceholder { get; set; }
        public String IdeaTextPlaceholder { get; set; }
        public String IdeaBenefitsPlaceholder { get; set; }
        public int UserComponent { get; set; }
        public int UserLocation { get; set; }
        public String Supervisor { get; set; }
        public List<SourceComponent> Components { get; set; }
        public List<Location> Locations { get; set; }
    }
}
