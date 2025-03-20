namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class IdeaFilter
    {
        public FilterType FilterType { get; set; }
        public bool Active { get; set; }
        public string DisplayText { get; set; }
        public string Title { get; set; }
        public IdeaFilter()
        {

        }

        public IdeaFilter(FilterType filterType, string displayText, string title) : this(filterType, displayText, title, false)
        {

        }

        public IdeaFilter(FilterType filterType, string displayText, string title, bool active)
        {
            FilterType = filterType;
            Active = active;
            DisplayText = displayText;
            Title = title;
        }
    }
}
