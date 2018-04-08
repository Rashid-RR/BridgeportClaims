using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public class PayorsDataProvider : IPayorsDataProvider
    {
        private readonly Lazy<IRepository<Payor>> _payorRepository;
        private readonly Lazy<IStoredProcedureExecutor> _storedProcedureExecutor;

        public PayorsDataProvider(
            Lazy<IRepository<Payor>> payorRepository, Lazy<IStoredProcedureExecutor> storedProcedureExecutor)
        {
            _payorRepository = payorRepository;
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public IEnumerable<Payor> GetAllPayors()
            => _payorRepository.Value.GetAll();

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
            return _storedProcedureExecutor.Value.ExecuteMultiResultStoredProcedure<PayorViewModel>(
                "EXEC [dbo].[uspGetPayors] :PageNumber, :PageSize",
                new List<SqlParameter> {pageNumberParam, pageSizeParam}).ToList();
        }
    }
}