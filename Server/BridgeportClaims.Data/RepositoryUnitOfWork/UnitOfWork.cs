using System;
using NHibernate;
using System.Data;

namespace BridgeportClaims.Data.RepositoryUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members

        private ITransaction _transaction;

        #endregion

        #region Ctor

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            CurrentSession = sessionFactory.OpenSession();
        }

        #endregion

        #region Public Members

        public ISession CurrentSession { get; private set; }

        #endregion

        #region Public Methods

        public void BeginTransaction()
        {
            try
            {
                if (null == _transaction || !_transaction.IsActive)
                {
                    _transaction = CurrentSession.BeginTransaction(IsolationLevel.ReadCommitted);
                }
            }
            catch
            {
                // rollback if there was an exception
                if (null != _transaction && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
        }

        public void Commit()
        {
            try
            {
                // commit transaction if there is one active
                if (null != _transaction && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                // rollback if there was an exception
                if (null != _transaction && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (null != _transaction && _transaction.IsActive)
                    _transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            CurrentSession.Dispose();
            CurrentSession = null;
            //GC.SuppressFinalize(this);
        }

        #endregion
    }
}