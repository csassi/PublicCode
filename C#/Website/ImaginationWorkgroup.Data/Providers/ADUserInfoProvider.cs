using CPI.DirectoryServices;
using ImaginationWorkgroup.Data.Configuration;
using ImaginationWorkgroup.Data.Entities;
using NHibernate.Loader;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Providers
{
    public class ADUserInfoProvider : IUserInfoProvider
    {
        internal enum FilterType { User, Group, None};

        public int RecordLimit { get; set; }

        private const string BindingPrefix = "LDAP://";
        private const string GlobalCatalogAddress = "GC://SOME_CATALOG";
        private static readonly string PHDomainAddress = $"{BindingPrefix}Some.domain.come";
        private const string RecursiveMemberOfSearch = ":1.2.840.113556.1.4.1941:";
        private const string CommonNameKey = "cn";


        public ADUserInfoProvider()
        {
            RecordLimit = 10;
        }

        public UserInfo FromEmail(string email)
        {
            this.Log().Debug($"Searching for email {email}");
            var filter = WrapAllFilterArguments(new[] { FormatFilterArgument(ADProperties.Email, email, false) });
            return SearchForSingle(filter);
        }

        public UserInfo FromPin(string pin)
        {
            this.Log().Debug($"Searching for pin {pin}");
            //in case the pin has the domain in it
            var parts = pin.Split(new[] { '\\' });
            if (parts.Length > 1)
                pin = parts.Last();
            var filter = WrapAllFilterArguments(new[] { FormatFilterArgument(ADProperties.SamAccount, pin, false) });
            return SearchForSingle(filter);
        }
        public IEnumerable<UserInfo> WildcardByName(string lastName)
        {
            return WildcardByName(lastName, null, null);
        }

        public IEnumerable<UserInfo> WildcardByEmail(string email)
        {
            return WildcardByName(null, null, email);
        }

        public IEnumerable<UserInfo> WildcardByName(string lastName, string firstName)
        {
            return WildcardByName(lastName, firstName, null);
        }
        public IEnumerable<UserInfo> WildcardByName(string lastName, string firstName, string email)
        {
            var employees = new List<UserInfo>();
            using (var ds = GetDirectorySearcher())
            {
                var filters = new List<string>();
                filters.Add(FormatFilterArgument(ADProperties.Surname, lastName, true));
                if (firstName != null)
                    filters.Add(FormatFilterArgument(ADProperties.FirstName, firstName, true));
                if (email != null)
                    filters.Add(FormatFilterArgument(ADProperties.Email, email, true));

                var filter = WrapAllFilterArguments(filters);
                ds.Filter = filter;

                var searchResults = ds.FindAll();
                this.Log().Info($"search for {ds.Filter} yields {searchResults.Count} results");
                foreach (SearchResult searchResult in searchResults)
                {
                    employees.Add(ExtractFromSearchResult(searchResult));
                }
            }
            return employees;
        }

        public IEnumerable<UserInfo> GetGroupMembers(string group)
        {
            var groupMembers = new List<UserInfo>();
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            var employees = new List<UserInfo>();
            using (var ds = GetDirectorySearcher())
            {
                var args = new[]
                {
                    FormatFilterArgument(ADProperties.SamAccount, group, false),
                };

                var filter = WrapAllFilterArguments(args, FilterType.Group);
                ds.Filter = filter;
                ds.PropertiesToLoad.Clear();
                ds.PropertiesToLoad.Add(ADProperties.Member);
                var resultGroup = ds.FindOne();
                if (resultGroup == null)
                    return new List<UserInfo>();

                using (var userDs = GetDirectorySearcher())
                {
                    foreach (var member in resultGroup.Properties[ADProperties.Member])
                    {
                        userDs.Filter = WrapAllFilterArguments(new[] {
                            FormatFilterArgument(ADProperties.DistinguishedName, member.ToString(), false)
                        }, FilterType.None);
                        var memberResult = userDs.FindOne();
                        if (memberResult != null)
                        {
                            //TODO: If the object category is a group, recursively trace that group to get the distinct user list
                            var objectCat = GetPropertyOrDefault(memberResult.Properties, ADProperties.ObjectCatKey);
                            if (objectCat.IndexOf(ADProperties.PersonCategory, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                groupMembers.Add(ExtractFromSearchResult(memberResult));
                            }
                        }
                    }

                }
            }
            return groupMembers;
        }


        [Obsolete("Do not use to get group membership due to cross-domain issues. Use IsUserInGroup instead to validate group membership")]
        public IEnumerable<string> GetAdGroupMembership(string pin)
        {
            var adGroups = new List<string>();
            using (var ds = GetDirectorySearcher())
            {
                ds.PropertiesToLoad.Clear();
                ds.PropertiesToLoad.Add(ADProperties.MemberOf);
                ds.Filter = WrapAllFilterArguments(new[] { FormatFilterArgument(ADProperties.SamAccount, pin, false) }, FilterType.User);
                var user = ds.FindOne();
                if (user != null)
                {
                    foreach (var grp in user.Properties[ADProperties.MemberOf])
                    {
                        var dn = new DN(grp.ToString());
                        var theCN = dn.RDNs.SelectMany(r => r.Components.Where(c => c.ComponentType == "CN")).FirstOrDefault();
                        if (theCN != null)
                            adGroups.Add(theCN.ComponentValue);
                    }
                }
            }
            return adGroups;
        }

        private string FormatFilterArgument(string key, string value, bool isWildcard)
        {
            this.Log().Debug($"building filter for {key}, {value} with wildcard={isWildcard}");
            if (isWildcard)
                value = $"{value}*";
            return $"({key}={value})";
        }

        private string WrapAllFilterArguments(IEnumerable<string> filters) 
        {
            return WrapAllFilterArguments(filters, FilterType.User);
        }

        private string WrapAllFilterArguments(IEnumerable<string> filters, FilterType filterType)
        {
            string category = filterType == FilterType.Group ? ADProperties.GroupCategory : ADProperties.PersonCategory;
           
            if(filterType == FilterType.Group || filterType == FilterType.User)
                filters = filters.Union(new[] { FormatFilterArgument(ADProperties.ObjectCatKey, category, false) });
            return $"(&{string.Join(string.Empty, filters)})";
        }

        private DirectorySearcher GetDirectorySearcher()
        {
            var forest = Forest.GetCurrentForest();
            var globalCatalog = forest.FindGlobalCatalog();
            var ds = globalCatalog.GetDirectorySearcher();
            ds.SizeLimit = RecordLimit;
            ds.PropertiesToLoad.Clear();
            ds.PropertiesToLoad.AddRange(ADProperties.DefaultSearchProperties);
            return ds;

        }
        

        private UserInfo ExtractFromSearchResult(SearchResult searchResult)
        {
            var ei = new UserInfo();

            if (searchResult.Properties.Count >= 1)
            {
                var prop = searchResult.Properties;
                ei.First = GetPropertyOrDefault(prop, ADProperties.FirstName);
                ei.Last = GetPropertyOrDefault(prop, ADProperties.Surname);
                ei.PrimaryPhone = GetPropertyOrDefault(prop, ADProperties.Phone);
                ei.UserPin = GetPropertyOrDefault(prop, ADProperties.SamAccount);
                ei.OfficeInfo = GetPropertyOrDefault(prop, ADProperties.Office);
                ei.Email = GetPropertyOrDefault(prop, ADProperties.Email);
            }
            return ei;
        }

        private string GetPropertyOrDefault(ResultPropertyCollection props, string key)
        {
            if (props.Contains(key))
            {
                var result = props[key];
                if (result.Count > 0)
                {
                    return result[0].ToString();
                }
            }
            return string.Empty;
        }
        private UserInfo SearchForSingle(string filter)
        {
            using (var ds = GetDirectorySearcher())
            {
                this.Log().Info($"Searching for single with filter {filter}");
                ds.Filter = filter;
                var searchResult = ds.FindOne();
                if (searchResult == null)
                {
                    this.Log().Info("Search yielded no results");
                    return UserInfo.NotFound;
                }
                return ExtractFromSearchResult(searchResult);
            }
        }

        public bool IsUserInGroup(string pin, string group)
        {
            this.Log().Info($"Checking group membership for {pin} in {group}");
            string userDN;
            DirectoryEntry groupEntry;
            using (var globalCatalog = new DirectoryEntry(GlobalCatalogAddress))
            using (var gcSearcher = new DirectorySearcher(globalCatalog))
            {
                //filter is users are not currently disabled (that's the weird 1.2.840... part)
                var userFilter = $"(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2)(sAMAccountName={pin}))";
                gcSearcher.Filter = userFilter;
                var userResult = gcSearcher.FindOne();
                if (userResult == null)
                {
                    this.Log().Info($"No directory entry found matching {userFilter}");
                    return false;
                }
                var dEntry = userResult.GetDirectoryEntry();
                userDN = dEntry.Properties["distinguishedName"].Value.ToString();
                this.Log().Info($"userdn found for {pin} - {userDN}");
            }

            //IWG has all groups in the PH domain
            //TODO: Look at caching the DNs of the groups in memory to directly bind instead of doing searches
            using (var phDomainEntry = new DirectoryEntry(PHDomainAddress))
            using (var groupSearcher = new DirectorySearcher(phDomainEntry))
            {
                var groupFilter = $"(&(objectCategory=group)(name={group}))";
                groupSearcher.Filter = groupFilter;
                var groupResult = groupSearcher.FindOne();
                if (groupResult == null)
                {
                    this.Log().Error($"No directory entry found matching {groupFilter} in {PHDomainAddress}");
                    return false;
                }
                groupEntry = groupResult.GetDirectoryEntry();
            }

            using (var groupMembershipSearcher = new DirectorySearcher(groupEntry))
            {
                //if this search yields any results, the user is in the group
                groupMembershipSearcher.SearchScope = SearchScope.Base;
                groupMembershipSearcher.Filter = $"(member{RecursiveMemberOfSearch}={userDN})";
                groupMembershipSearcher.PropertiesToLoad.Add(CommonNameKey);  //minimal attributes
                var groupResult = groupMembershipSearcher.FindOne();
                bool inGroup = groupResult != null;
                this.Log().Info($"User {pin} in group {groupEntry.Properties["DistinguishedName"].Value.ToString()} result: {inGroup}");
                return inGroup;
            }

        }
    }
}
