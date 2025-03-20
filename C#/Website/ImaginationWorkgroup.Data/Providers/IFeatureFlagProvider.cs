using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Providers
{
    public interface IFeatureFlagProvider
    {
        bool CreateIdeaView { get; }
        bool ListIdeasView { get; }
        bool RedirectToAbout { get; }
        bool ShowComingSoonOnAbout { get; }
        bool ViewIdeaDetails { get; }
        bool ShowCreator { get; }
    }
}
