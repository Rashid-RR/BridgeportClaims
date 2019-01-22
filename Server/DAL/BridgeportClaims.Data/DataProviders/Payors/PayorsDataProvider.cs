using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using SQLinq;
using Dapper;
using SQLinq.Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public class PayorsDataProvider : IPayorsDataProvider
    {
        public IEnumerable<PayorDto> GetPayors() => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query(new SQLinq<PayorDto>().OrderBy(c => c.Carrier)
                    ?.Select(c => new {c.PayorId, c.Carrier}));
            });

        public IEnumerable<PayorFullDto> GetAllPayors()
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPayors]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<PayorFullDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public IList<PayorFullDto> GetPaginatedPayors(int pageNumber, int pageSize) =>
            GetAllPayors()?.ToList();

        public IEnumerable<PayorDto> GetPayors(string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPayorsByCollectionAssignment]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<PayorDto>(sp, new {UserID = userId}, commandType: CommandType.StoredProcedure)
                    ?.OrderBy(o => o.Carrier);
            });

        public PayorListDto GetPayorList(string searchText, int page, int pageSize, string sort, string sortDirection)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPayorList]";
                const string totalRows = "@TotalRows";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@SearchText", searchText, DbType.AnsiString, size: 4000);
                ps.Add("@SortColumn", sort, DbType.AnsiString, size: 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, size: 5);
                ps.Add("@PageNumber", page, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add(totalRows, DbType.Int32, direction: ParameterDirection.Output);
                var query = conn.Query<PayorResultDto>(sp, ps, commandType: CommandType.StoredProcedure);
                var adj = new PayorListDto
                {
                    Results = query,
                    TotalRowCount = ps.Get<int>(totalRows)
                };
                return adj;
            });
    }
}