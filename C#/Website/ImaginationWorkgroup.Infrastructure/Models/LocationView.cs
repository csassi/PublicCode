namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class LocationView : ViewModelBase
    {
        public string Location { get; set; }
        public LocationView(int id, string location)
        {
            Location = location;
            Id = id;
        }
    }

}
