using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Linq;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using BridgeportClaims.Entities.Domain;

namespace BridgeportClaims.Data.Repositories
{
    /// <summary>
    /// Implementation of the IRepository pattern. No logging done at this level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : BaseRepository, IRepository<T> where T : class, IEntity, new()
    {
        private readonly UnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = (UnitOfWork) unitOfWork;
        }
        public T Load(int id) => Session.Load<T>(id);
        public T Load(object obj) => Session.Load<T>(obj);

        public T Get(Expression<Func<T, bool>> predicate) => Session.Query<T>().Where(predicate).FirstOrDefault();

        public T Get(object id) => Session.Get<T>(id);

        public void Save(T value)
        {
            Session.Save(value);
        }

        public void SaveOrUpdateMany(IEnumerable<T> values)
        {
            foreach (var value in values)
                Session.SaveOrUpdate(value);
        }

        public void SaveOrUpdate(T value)
        {
            Session.SaveOrUpdate(value);
        }

        public void Update(T value)
        {
            Session.Update(value);
        }

        public void Delete(T value)
        {
            Session.Delete(value);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
            => Session.Query<T>().Where(predicate);


        public IQueryable<T> GetAll() => Session.Query<T>();


        public IQueryable<T> GetTop(int top) => Session.Query<T>().Select(q => q).Take(top);
    }
}