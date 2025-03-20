using System.Collections.Generic;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public class LocationChangeModel
    {
        public bool CanChangeLocation { get; set; }
        public LocationView CurrentLocation { get; set; }
        public List<LocationView> PossibleLocations { get; set; }
    }
}
