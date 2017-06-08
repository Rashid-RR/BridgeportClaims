using NHibernate;
using System;
using System.Data;
using BridgeportClaims.Data.SessionFactory;

namespace BridgeportClaims.Data.RepositoryUnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private static readonly ISessionFactory SessionFactory;
        private readonly ITransaction _transaction;

        public ISession Session { get; private set; }

        #region Ctors

        static UnitOfWork()
        {
            SessionFactory = SessionFactoryBuilder.GetSessionFactory();
        }

        public UnitOfWork(ITransaction transaction)
        {
            _transaction = transaction;
            Session = SessionFactory.OpenSession();
        }

        #endregion

        public void BeginTransaction()
        {
            try
            {
                Session.BeginTransaction(IsolationLevel.ReadCommitted); // READ_COMMITTED_SNAPSHOT
            }
            finally
            {
                Session.Dispose();
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
                Session.Dispose();
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
                Session.Dispose();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
            Session = null;
            //GC.SuppressFinalize(this);
        }
    }
}