﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        IQueryable<T> GetTop(int top);
    }
}