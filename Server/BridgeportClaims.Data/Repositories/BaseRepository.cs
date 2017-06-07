using System;
using NHibernate;

namespace BridgeportClaims.Data.Repositories
{
    public class BaseRepository
    {
        protected ISession Session { get; set; }
        
        public BaseRepository() { }

        public BaseRepository(ISession session)
        {
            if (null == session)
                throw new ArgumentNullException(nameof(session), "No Nhibernate Session was supplied to the provider");
                Session = session;
        }

        public bool IsConfigured() => null != Session;
    }
}
