using System.Collections.Generic;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class IdeaListModel
    {
        public IEnumerable<IdeaListItem> ListItems { get; set; }
        public bool IsReviewer { get; set; }
        public bool ViewIdeaDetails { get; set; }

        public IdeaListModel()
        {
            ViewIdeaDetails = true;
        }

        public IdeaListModel(IEnumerable<IdeaListItem> listItems, bool isReviewer, bool viewIdeaDetails)
        {
            ListItems = listItems;
            IsReviewer = isReviewer;
            ViewIdeaDetails = viewIdeaDetails;
        }
    }
}
