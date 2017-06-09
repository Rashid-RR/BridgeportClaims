using System;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using NHibernate;

namespace BridgeportClaims.Data.Repositories
{
    public class BaseRepository
    {
        private readonly UnitOfWork _unitOfWork;
        private ISession _session;

        protected ISession Session
        {
            get
            {
                _session = _unitOfWork.CurrentSession;
                return _session;
            }
            private set { _session = value; }
        }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public BaseRepository(ISession session)
        {
            if (null == session)
                throw new ArgumentNullException(nameof(session), "No Nhibernate CurrentSession was supplied to the provider");
            Session = session;
        }

        public bool IsConfigured() => null != Session;
    }
}
