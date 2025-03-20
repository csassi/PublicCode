using ImaginationWorkgroup.Data.Configuration;
using NHibernate;
using System;
using System.Configuration;
using System.Linq;

namespace ImaginationWorkgroup.Data.Repositories
{
    public class Repository : IRepository, IDisposable
    {
        private static ISessionFactory _sessionFactory;
        private ISession _session;

        static Repository()
        {
            var cs = ConfigurationManager.ConnectionStrings["Imagination"].ConnectionString;
            _sessionFactory = NhConfigurator.GetSessionFactory(cs);
        }

        public Repository()
        {
            _session = _sessionFactory.OpenSession();
            _session.FlushMode = FlushMode.Auto;
        }

        public void Add<T>(T record)
        {
            _session.Save(record);
        }

        public void Delete<T>(T record)
        {
            _session.Delete(record);
        }

        public T Get<T>(object id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        public void Update<T>(T record)
        {
            _session.Update(record);
            _session.Flush();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _session.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
