using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace BridgeportClaims.Data.Repositories
{
    /// <summary>
    /// Implementation of the IRepository pattern. No logging done at this level.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : BaseRepository, IRepository<TEntity> where TEntity : class, new()
    {
        public Repository(ISession session) : base(session) { }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate) => Session.Query<TEntity>().Where(predicate).FirstOrDefault();

        public TEntity Get(object id) => Session.Get<TEntity>(id);
        public TEntity Load(object id) => Session.Load<TEntity>(id);

        public void Save(TEntity value)
        {
            Session.Save(value);
        }

        public void SaveOrUpdateMany(IEnumerable<TEntity> values)
        {
            foreach (var value in values)
                Session.SaveOrUpdate(value);
        }

        public void SaveOrUpdate(TEntity value)
        {
            Session.SaveOrUpdate(value);
        }

        public void Update(TEntity value)
        {
            Session.Update(value);
        }

        public void Delete(TEntity value)
        {
            Session.Delete(value);
        }

        public IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate)
            => Session.Query<TEntity>().Where(predicate);


        public IQueryable<TEntity> GetAll() => Session.Query<TEntity>();


        public IQueryable<TEntity> GetTop(int top) => Session.Query<TEntity>().Select(q => q).Take(top);
    }
}