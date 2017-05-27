using System;
using System.Linq;
using System.Linq.Expressions;
using BridgeportClaims.Entities.DomainModels;
using System.Collections.Generic;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Data.Services.Payors
{
    public interface IPayorService
    {
        Payor GetPayorById(int id);
        IQueryable<Payor> GetManyPayors(Expression<Func<Payor, bool>> predicate);
        IQueryable<Payor> GetAllPayors();
        IQueryable<Payor> GetTopPayors(int top);
        IList<PayorViewModel> GetPaginatedPayors(int pageNumber, int pageSize);
        void InsertPayor(Payor payor);
        void UpdatePayor(Payor payor);
        void DeletePayor(Payor payor);
        void DeletePayorByPayorId(int id);
    }
}