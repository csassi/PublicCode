using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Configuration
{
    public class ADProperties
    {
        public static readonly string ObjectCatKey = "objectCategory";
        public static readonly string PersonCategory = "person";
        public static readonly string GroupCategory = "group";
        public static readonly string SamAccount = "samaccountname";
        public static readonly string Surname = "sn";
        public static readonly string FirstName = "givenName";
        public static readonly string Email = "mail";
        public static readonly string Office = "physicalDeliveryOfficeName";
        public static readonly string Phone = "telephoneNumber";
        public static readonly string Member = "member";
        public static readonly string DistinguishedName = "distinguishedName";
        public static readonly string MemberOf = "memberOf";

        public static string[] DefaultSearchProperties = new string[] { SamAccount, Surname, FirstName, Email, Office, Phone, ObjectCatKey};
        
        public static string[] DefaultGroupProperties = new string[] { Member };
        
    }
}
