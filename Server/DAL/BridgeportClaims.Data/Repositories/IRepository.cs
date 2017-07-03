using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BridgeportClaims.Data.Repositories
{
    /// <summary>
    /// Should you ever need to add functionality specific to a single class, extend
    /// the interface.
    /// </summary>
    public interface IRepository<T> where T : class, new()
    {
        T Get(object id);
        T Load(object id);
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        T GetSingleOrDefault(Expression<Func<T, bool>> predicate);
        void Save(T value);
        void SaveOrUpdateMany(IEnumerable<T> values);
        void SaveOrUpdate(T value);
        void Update(T value);
        void Delete(T value);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetTop(int top);
    }
}