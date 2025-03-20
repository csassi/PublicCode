using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Repositories;
using System;
using System.Linq;

namespace ImaginationWorkgroup.Data.Providers
{
    public class FeatureFlagProvider : IFeatureFlagProvider
    {
        private IRepository _repo;
        public FeatureFlagProvider(IRepository repo)
        {
            _repo = repo;
        }


        public bool CreateIdeaView => IsEnabled("CreateIdeas");

        public bool ListIdeasView => IsEnabled("ListIdeas");

        public bool RedirectToAbout => IsEnabled("RedirectToAbout");

        public bool ShowComingSoonOnAbout => IsEnabled("ShowComingSoonMessages");

        public bool ViewIdeaDetails => IsEnabled("ViewIdeaDetails");
        public bool ShowCreator => IsEnabled("ShowCreator");

        private bool IsEnabled(string key)
        {
            return IsEnabled(key, false);
        }
        private bool IsEnabled(string key, bool defaultValue)
        {
            try
            {
                var feature = _repo.Query<FeatureFlag>().FirstOrDefault(ff => ff.Feature == key);
                return feature?.Active ?? defaultValue;
            }
            catch(Exception ex)
            {
                this.Log().Error(ex.ToString());
                return defaultValue;
            }
        }
    }
}
