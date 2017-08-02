using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BridgeportClaims.Common.Disposable;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Data.DataProviders.Payments
{
    public class PaymentsDataProvider : IPaymentsDataProvider
    {
        public byte[] GetBytesFromDbAsync(string fileName) => DisposableService.Using(() 
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

        public async Task ImportDataTableIntoDbAsync(DataTable dt) => await DisposableService.Using(() 
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