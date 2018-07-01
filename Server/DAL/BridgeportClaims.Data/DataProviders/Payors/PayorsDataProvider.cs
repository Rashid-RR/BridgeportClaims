using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public class PayorsDataProvider : IPayorsDataProvider
    {
        private readonly Lazy<IRepository<Payor>> _payorRepository;
        private readonly Lazy<IStoredProcedureExecutor> _storedProcedureExecutor;
        private const string Query = "SELECT TOP(30) p.PayorID PayorId, p.GroupName Carrier FROM dbo.Payor AS p";

        public PayorsDataProvider(
            Lazy<IRepository<Payor>> payorRepository, Lazy<IStoredProcedureExecutor> storedProcedureExecutor)
        {
            _payorRepository = payorRepository;
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public IEnumerable<PayorDto> GetPayors() => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                conn.Open();
                var query = conn.Query<PayorDto>(Query, commandType: CommandType.Text);
                return query;
            });

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