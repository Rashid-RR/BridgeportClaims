using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public class PayorsDataProvider : IPayorsDataProvider
    {
        private const string Query = "SELECT p.PayorID PayorId, p.GroupName Carrier FROM dbo.Payor AS p";

        public IEnumerable<PayorDto> GetPayors() => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                conn.Open();
                var query = conn.Query<PayorDto>(Query, commandType: CommandType.Text);
                return query?.OrderBy(x => x.Carrier);
            });

        public IEnumerable<PayorFullDto> GetAllPayors()
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPayors]";
                conn.Open();
                return conn.Query<PayorFullDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public IList<PayorFullDto> GetPaginatedPayors(int pageNumber, int pageSize) =>
            GetAllPayors()?.ToList();
    }
}