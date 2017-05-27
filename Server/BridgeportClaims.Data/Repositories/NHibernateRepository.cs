using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using BridgeportClaims.Common.ExpressionHelpers;

namespace BridgeportClaims.Data.Repositories
{
    /// <summary>
    /// Implementation of the IRepository pattern. No logging done at this level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateRepository<T> : BaseRepository, IRepository<T> where T : class, new()
    {
        public NHibernateRepository(ISession session) : base(session) { }
        private const string TranNotCommittedCorrectly = "The transaction was not committed properly.";

        public T Load(object id) => DisposableHelper.Using(
            () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
            {
                try
                {
                    var returnVal = Session.Load<T>(id);
                    if (transaction.IsActive)
                        transaction.Commit();
                    if (!transaction.WasCommitted)
                        throw new Exception(TranNotCommittedCorrectly);
                    return returnVal;
                }
                catch (Exception)
                {
                    if (transaction.IsActive)
                        transaction.Rollback();
                    throw;
                }
            });

        public T Get(Expression<Func<T, bool>> predicate) => DisposableHelper.Using(
                () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
                {
                    try
                    {
                        var returnVal = Session.Query<T>().Where(predicate).FirstOrDefault();
                        if (transaction.IsActive)
                            transaction.Commit();
                        return returnVal;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }

                });


        public T Get(object id) => DisposableHelper.Using(
            () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
            {
                try
                {
                    var returnVal = Session.Get<T>(id);
                    if (transaction.IsActive)
                        transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            });
        

        public void Save(T value)
        {
            using (var transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Session.Save(value);
                    if (transaction.IsActive)
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
            using (var transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    foreach(var value in values)
                        Session.SaveOrUpdate(value);
                    if (transaction.IsActive)
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
            using (var transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Session.SaveOrUpdate(value);
                    if (transaction.IsActive)
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
            using (var transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Session.Update(value);
                    if (transaction.IsActive)
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
            using (var transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Session.Delete(value);
                    if (transaction.IsActive)
                        transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate) => DisposableHelper.Using(
            () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
            {
                try
                {
                    var returnVal = Session.Query<T>().Where(predicate);
                    if (transaction.IsActive)
                        transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });


        public IQueryable<T> GetAll() => DisposableHelper.Using(
            () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
            {
                try
                {
                    var returnVal = Session.Query<T>();
                    if (transaction.IsActive)
                        transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            });


        public IQueryable<T> GetTop(int top) => DisposableHelper.Using(
            () => Session.BeginTransaction(IsolationLevel.ReadCommitted), transaction =>
            {
                try
                {
                    var returnVal = Session.Query<T>().Select(q => q).Take(top);
                    if (transaction.IsActive)
                        transaction.Commit();
                    return returnVal;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            });
    }
}
