using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public class PayorService : IPayorService
    {
        private readonly IRepository<Payor> _payorRepository;
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;

        public PayorService(IRepository<Payor> payorRepository, IStoredProcedureExecutor storedProcedureExecutor)
        {
            _payorRepository = payorRepository;
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public Payor GetPayorById(int id) => _payorRepository.Get(id);

        public IEnumerable<Payor> GetManyPayors(Expression<Func<Payor, bool>> predicate) =>
            _payorRepository.GetMany(predicate);

        public IEnumerable<Payor> GetAllPayors()
            => _payorRepository.GetAll();

        public IEnumerable<Payor> GetTopPayors(int top) => _payorRepository.GetTop(top);

        public IList<PayorViewModel> GetPaginatedPayors(int pageNumber, int pageSize)
        {
            var pageNumberParam = new SqlParameter
            {
                ParameterName = "PageNumber",
                Value = pageNumber,
                DbType = DbType.Int32
            };
            var pageSizeParam = new SqlParameter
            {
                ParameterName = "PageSize",
                Value = pageSize,
                DbType = DbType.Int32
            };
            return _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<PayorViewModel>(
                "EXEC [dbo].[uspGetPayors] :PageNumber, :PageSize",
                new List<SqlParameter> {pageNumberParam, pageSizeParam}).ToList();
        }

        public void InsertPayor(Payor payor)
            => _payorRepository.Save(payor);

        public void UpdatePayor(Payor payor)
            => _payorRepository.Update(payor);

        public void DeletePayor(Payor payor)
            => _payorRepository.Delete(payor);

        public void DeletePayorByPayorId(int id)
            => _payorRepository.Delete(GetPayorById(id));
    }
}