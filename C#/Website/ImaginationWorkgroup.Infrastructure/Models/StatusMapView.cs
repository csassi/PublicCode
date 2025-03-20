using ImaginationWorkgroup.Data.Entities;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class StatusMapView
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayText { get; set; }
        public string Title { get; set; }
        public string ButtonClass { get; set; }
        public string PromptText { get; set; }
        public bool RequireComments { get; set; }
        public string PromptPlaceholder { get; set; }
        public StatusMapView()
        {

        }

        public StatusMapView(StatusMap statusMap)
        {
            var nMap = statusMap.NextMap;
            Id = statusMap.Id;
            DisplayOrder = statusMap.DisplayOrder;
            DisplayText = nMap.DisplayText;
            Title = nMap.Title;
            ButtonClass = nMap.ButtonClass;
            PromptText = nMap.PromptText;
            RequireComments = nMap.RequireComments;
            PromptPlaceholder = nMap.PromptPlaceholder;
        }

    }
}
