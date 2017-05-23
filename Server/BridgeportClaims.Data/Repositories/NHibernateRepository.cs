using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace BridgeportClaims.Data.Repositories
{
    /// <summary>
    /// Implementation of the IRepository pattern. No logging done at this level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateRepository<T> : BaseRepository, IRepository<T> where T : class, new()
    {
        public NHibernateRepository(ISession session) : base(session) { }

        public T Load(object id)
        {
            /*return
                DisposableHelper.Using(
                    () => Session.BeginTransaction(),
                    transaction =>
                    {
                        try
                        {
                            T returnVal = Session.Load<T>(id);
                            if (transaction.IsActive)
                                transaction.Commit();
                            if (!transaction.WasCommitted)
                                throw new Exception("The transaction was not committed properly.");
                            return returnVal;
                        }
                        catch (Exception)
                        {
                            if (transaction.IsActive)
                                transaction.Rollback();
                            throw;
                        }
                    });*/
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    var returnVal = Session.Load<T>(id);
                    transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    var returnVal = Session.Query<T>().Where(predicate).FirstOrDefault();
                    transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public T Get(object id)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    var returnVal = Session.Get<T>(id);
                    transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public void Save(T value)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    Session.Save(value);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public void SaveOrUpdateMany(IEnumerable<T> values)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    foreach(var value in values)
                        Session.SaveOrUpdate(value);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void SaveOrUpdate(T value)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    Session.SaveOrUpdate(value);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Update(T value)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    Session.Update(value);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public void Delete(T value)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    Session.Delete(value);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    var returnVal = Session.Query<T>().Where(predicate);
                    transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IQueryable<T> GetAll()
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.Snapshot))
            {
                try
                {
                    var returnVal = Session.Query<T>();
                    transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }
    }
}
