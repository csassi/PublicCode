using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Infrastructure.Models;
using System.Collections.Generic;

namespace ImaginationWorkgroup.Infrastructure.QueryHelpers
{
    public interface IIdeaFilterAppender
    {
        IEnumerable<Idea> ApplyFilters(IEnumerable<Idea> baseEnumerable, FilterType filterType, string user);
    }
}
