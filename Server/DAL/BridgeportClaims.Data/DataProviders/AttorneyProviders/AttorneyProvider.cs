using System.Data;
using System.Data.SqlClient;
using System.Linq;
using cs = BridgeportClaims.Common.Config.ConfigService;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.AttorneyProviders
{
    public class AttorneyProvider : IAttorneyProvider
    {
        public AttorneyDto GetAttorneys(string searchText, int page, int pageSize, string sort, string sortDirection)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
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

        public AttorneyResultDto InsertAttorney(string attorneyName, string address1, string address2, string city,
            int stateId, string postalCode, string phoneNumber, string faxNumber, string modifiedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspAttorneyInsert]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@AttorneyName", attorneyName, DbType.AnsiString, size: 255);
                ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                ps.Add("@City", city, DbType.AnsiString, size: 255);
                ps.Add("@StateID", stateId, DbType.Int32);
                ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 255);
                ps.Add("@PhoneNumber", phoneNumber, DbType.AnsiString, size: 30);
                ps.Add("@FaxNumber", faxNumber, DbType.AnsiString, size: 30);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                var attorneyResult =
                    conn.Query<AttorneyResultDto>(sp, ps, commandType: CommandType.StoredProcedure)?.SingleOrDefault();
                return attorneyResult;
            });

        public AttorneyResultDto UpdateAttorney(int attorneyId, string attorneyName, string address1,
            string address2, string city, int stateId, string postalCode, string phoneNumber,
            string faxNumber, string userId) => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspAttorneyUpdate]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@AttorneyID", attorneyId, DbType.Int32);
                ps.Add("@AttorneyName", attorneyName, DbType.AnsiString, size: 255);
                ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                ps.Add("@City", city, DbType.AnsiString, size: 255);
                ps.Add("@StateID", stateId, DbType.Int32);
                ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 255);
                ps.Add("@PhoneNumber", phoneNumber, DbType.AnsiString, size: 30);
                ps.Add("@FaxNumber", faxNumber, DbType.AnsiString, size: 30);
                ps.Add("@ModifiedByUserID", userId, DbType.String, size: 128);
                var attorneyResult =
                    conn.Query<AttorneyResultDto>(sp, ps, commandType: CommandType.StoredProcedure)?.SingleOrDefault();
                return attorneyResult;
            });
    }
}