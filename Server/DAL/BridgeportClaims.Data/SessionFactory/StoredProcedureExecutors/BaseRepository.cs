using System;
using NHibernate;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public class BaseRepository
    {
        private readonly ISession _session;
        public BaseRepository(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session), "No NHibernate session argument was supplied");
        }
        protected ISession Session
        {
            get
            {
                if (null != _session)
                    return _session;
                throw new Exception("Session object not initialized by Ninject");
            }
        }
        public bool IsConfigured() => null != _session;
    }
}
