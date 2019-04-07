using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.InvoicePdfDocuments
{
    public class InvoicePdfDocumentProvider : IInvoicePdfDocumentProvider
    {
        public InvoicePdfDto GetInvoicePdfDocument() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetInvoicingPdfData]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InvoicePdfDto>(sp, commandType: CommandType.StoredProcedure)?.SingleOrDefault();
            });
    }
}