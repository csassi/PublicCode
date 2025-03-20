using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Entities
{
    public class FeatureUtilization : ImaginationEntityBase
    {
        [Required]
        [StringLength(500)]
        public virtual string Pathname { get; set; }
        [StringLength(500)]
        public virtual string UriQuery { get; set; }
        [StringLength(3000)]
        public virtual string Payload { get; set; }
        [StringLength(15)]
        public virtual string UserPin { get; set; }

        public FeatureUtilization()
        {

        }

        public FeatureUtilization(string pathname, string uriQuery, string payload, string userPin)
        {
            Pathname = pathname;
            UriQuery = uriQuery;
            Payload = payload;
            UserPin = userPin;
        }
    }
}
