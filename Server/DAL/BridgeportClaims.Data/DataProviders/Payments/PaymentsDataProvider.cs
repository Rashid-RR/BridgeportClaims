using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.Common.Disposable;
using c = BridgeportClaims.Common.StringConstants.Constants;
using BridgeportClaims.Excel.Adapters;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public class PaymentsDataProvider : IPaymentsDataProvider
    {
        public async Task ImportPaymentFile(string fileName)
        {
            var fileBytes = GetBytesFromDbAsync(fileName);
            var dt = OleDbExcelAdapter.GetDataTableFromExcel(fileBytes.ToArray(), true);
            await ImportDataTableIntoDbAsync(dt);
        }

        private static IEnumerable<byte> GetBytesFromDbAsync(string fileName) => DisposableService.Using(() 
            => new SqlConnection(c.ConnStr), conn =>
            {
                conn.Open();
                return DisposableService.Using(() => new SqlCommand("uspGetFileBytesFromFileName", conn),
                    cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileName;
                        return DisposableService.Using(cmd.ExecuteReader,
                            reader =>
                            {
                                if (reader.Read())
                                    return (byte[])reader["FileBytes"];
                                throw new Exception("Unable to read from the SqlDataReader");
                            });
                    });
            });

        private static async Task ImportDataTableIntoDbAsync(DataTable dt) => await DisposableService.Using(() 
            => new SqlConnection(c.ConnStr), async conn =>
            {
                conn.Open();
                await DisposableService.Using(() => new SqlCommand("uspImportPaymentFromDataTable", conn),
                    async cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var dataTableParam = new SqlParameter
                        {
                            Value = dt,
                            SqlDbType = SqlDbType.Structured,
                            ParameterName = "@Payment"
                        };
                        cmd.Parameters.Add(dataTableParam);
                        await cmd.ExecuteNonQueryAsync();
                    });
            });
    }
}