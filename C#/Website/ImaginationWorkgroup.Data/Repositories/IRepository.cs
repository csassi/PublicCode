using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Data.Repositories
{
    public interface IRepository
    {
        void Add<T>(T record);
        void Delete<T>(T record);
        void Update<T>(T record);
        T Get<T>(object id);
        IQueryable<T> Query<T>();
    }
}
