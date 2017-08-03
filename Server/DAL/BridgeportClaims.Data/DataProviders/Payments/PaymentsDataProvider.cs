using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.DataTables;
using BridgeportClaims.Common.Disposable;
using c = BridgeportClaims.Common.StringConstants.Constants;
using BridgeportClaims.Excel.Adapters;

namespace BridgeportClaims.Data.DataProviders.Payments
{
	public class PaymentsDataProvider : IPaymentsDataProvider
	{
		public void ImportPaymentFile(string fileName)
		{
			var fileBytes = GetBytesFromDbAsync(fileName);
			var dt = ExcelDataReaderAdapter.ReadExcelFileIntoDataTable(fileBytes.ToArray());
			var newDt = DataTableProvider.FormatDataTableForPaymentImport(dt, true);
			ImportDataTableIntoDbAsync(newDt);
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

		private static void ImportDataTableIntoDbAsync(DataTable dt) => DisposableService.Using(() 
			=> new SqlConnection(c.ConnStr), conn =>
			{
				DisposableService.Using(() => new SqlCommand("uspImportPaymentFromDataTable", conn),
					cmd =>
					{
						conn.Open();
						cmd.CommandType = CommandType.StoredProcedure;
						var dataTableParam = new SqlParameter
						{
							Value = dt,
							SqlDbType = SqlDbType.Structured,
							ParameterName = "@Payment"
						};
						cmd.Parameters.Add(dataTableParam);
						cmd.ExecuteNonQuery();

					});
			});
	}
}