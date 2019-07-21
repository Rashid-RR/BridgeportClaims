using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.InvoicesProvider
{
    public class InvoicesProvider : IInvoicesProvider
    {
        public IEnumerable<InvoiceDto> GetInvoices() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspInvoicesScreen]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InvoiceDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public IEnumerable<InvoiceProcessDto> GetInvoiceProcesses() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspInvoicesProcess]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InvoiceProcessDto>(sp, commandType: CommandType.StoredProcedure);
            });
    }
}