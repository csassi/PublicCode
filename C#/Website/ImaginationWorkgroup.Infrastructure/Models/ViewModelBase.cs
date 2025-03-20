using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models
{
    public abstract class ViewModelBase
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }

        public string CreatedLocal => Created.ToLocalTime().DateTime.ToString();
        public string ModifiedLocal => Modified.ToLocalTime().DateTime.ToString();

        public string CreatedFormatted => Created.ToString("yyyy/MM/dd");
        public string ModifiedFormatted => Modified.ToString("yyyy/MM/dd");

        public ViewModelBase()
        {

        }

        protected ViewModelBase(int id, DateTimeOffset created, DateTimeOffset modified)
        {
            Id = id;
            Created = created;
            Modified = modified;
        }
    }
}
