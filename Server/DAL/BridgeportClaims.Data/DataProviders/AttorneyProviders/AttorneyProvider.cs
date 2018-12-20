using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.AttorneyProviders
{
    public class AttorneyProvider : IAttorneyProvider
    {
        public AttorneyDto GetAttorneys(string searchText, int page, int pageSize, string sort, string sortDirection)
            => DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspGetAttorneys]";
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
                ps.Add(totalRows, totalRows, DbType.Int32, ParameterDirection.Output);
                var query = conn.Query<AttorneyResultDto>(sp, ps, commandType: CommandType.StoredProcedure);
                var attorneyDto = new AttorneyDto
                {
                    Results = query,
                    TotalRows = ps.Get<int>(totalRows)
                };
                return attorneyDto;
            });
    }
}