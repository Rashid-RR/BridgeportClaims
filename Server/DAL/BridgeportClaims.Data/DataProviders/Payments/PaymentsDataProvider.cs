using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.DataTables;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.StoredProcedureExecutors;
using c = BridgeportClaims.Common.StringConstants.Constants;
using BridgeportClaims.Excel.Adapters;

namespace BridgeportClaims.Data.DataProviders.Payments
{
	public class PaymentsDataProvider : IPaymentsDataProvider
	{
		private readonly IStoredProcedureExecutor _storedProcedureExecutor;

		public PaymentsDataProvider(IStoredProcedureExecutor storedProcedureExecutor)
		{
			_storedProcedureExecutor = storedProcedureExecutor;
		}

		public IList<PaymentDto> SearchPayments(string claimNumber, string firstName,
			string lastName, DateTime? rxDate, string invoiceNumber)
		{
		    var claimNumberParam = new SqlParameter {ParameterName = "ClaimNumber", Value = claimNumber, DbType = DbType.String };
		    var firstNameParam = new SqlParameter { ParameterName = "FirstName", Value = firstName, DbType = DbType.String };
		    var lastNameParam = new SqlParameter { ParameterName = "LastName", Value = lastName, DbType = DbType.String };
		    var rxDateParam = new SqlParameter { ParameterName = "RxDate", Value = lastName, DbType = DbType.String };
		    var invoiceNumberParam = new SqlParameter { ParameterName = "InvoiceNumber", Value = lastName, DbType = DbType.String };
		    var paymentDtos = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<PaymentDto>(
		        "EXEC dbo.uspGetPaymentPageData @ClaimNumber = :ClaimNumber, @FirstName = :FirstName, " +
		        "@LastName = :LastName, @RxDate = :RxDate, @InvoiceNumber = :InvoiceNumber", new List<SqlParameter>
		            {claimNumberParam, firstNameParam, lastNameParam, rxDateParam, invoiceNumberParam})?.ToList();
		    return paymentDtos;
		}

		public void ImportPaymentFile(string fileName)
		{
			var fileBytes = GetBytesFromDbAsync(fileName);
			var dt = ExcelDataReaderAdapter.ReadExcelFileIntoDataTable(fileBytes.ToArray());
			var newDt = DataTableProvider.FormatDataTableForPaymentImport(dt, true);
			ImportDataTableIntoDbAsync(newDt);
		}

		public IList<PaymentSearchResultsDto> GetInitialPaymentsSearchResults(string claimNumber, string firstName,
			string lastName, DateTime? rxDate, string invoiceNumber)
		{
			var paymentSearchResultsDtos = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<PaymentSearchResultsDto>(
				"EXEC dbo.uspGetPaymentSearchResults @ClaimNumber = :ClaimNumber, @FirstName = :FirstName, " +
				"@LastName = :LastName, @RxDate = :RxDate, @InvoiceNumber = :InvoiceNumber", new List<SqlParameter>
				{
					new SqlParameter
					{
						ParameterName = "ClaimNumber",
						Value = claimNumber,
						DbType = DbType.String
					},
					new SqlParameter
					{
						ParameterName = "FirstName",
						Value = firstName,
						DbType = DbType.String
					},
					new SqlParameter
					{
						ParameterName = "LastName",
						Value = lastName,
						DbType = DbType.String
					},
					new SqlParameter
					{
						ParameterName = "RxDate",
						Value = rxDate?.ToShortDateString(),
						DbType = DbType.String
					},
					new SqlParameter
					{
						ParameterName = "InvoiceNumber",
						Value = invoiceNumber,
						DbType = DbType.String
					}
				})?.ToList();
			return paymentSearchResultsDtos;
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