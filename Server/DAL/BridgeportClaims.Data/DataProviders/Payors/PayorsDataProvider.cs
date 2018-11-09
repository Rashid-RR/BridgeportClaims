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
    }
}