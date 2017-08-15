using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using BridgeportClaims.Data.Dtos;
using System.Collections.Generic;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Excel.Adapters;
using BridgeportClaims.Common.DataTables;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.StoredProcedureExecutors;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Data.DataProviders.Payments
{
	public class PaymentsDataProvider : IPaymentsDataProvider
	{
		private readonly IStoredProcedureExecutor _storedProcedureExecutor;
	    private readonly IMemoryCacher _memoryCacher;

		public PaymentsDataProvider(IStoredProcedureExecutor storedProcedureExecutor, IMemoryCacher memoryCacher)
		{
		    _storedProcedureExecutor = storedProcedureExecutor;
		    _memoryCacher = memoryCacher;
		}

		public IList<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(IList<int> claimIds)
		{
			var splitClaimIds = string.Join(c.Comma, claimIds);
			var claimIdParam = new SqlParameter {ParameterName = "ClaimIDs", Value = splitClaimIds, DbType = DbType.String };
			var paymentSearchResultsDtos = _storedProcedureExecutor
				.ExecuteMultiResultStoredProcedure<ClaimsWithPrescriptionDetailsDto>(
					"EXEC [dbo].[uspGetClaimsWithPrescriptionDetails] @ClaimIDs = :ClaimIDs",
					new List<SqlParameter> {claimIdParam});
			return paymentSearchResultsDtos?.ToList();
		}

		public void ImportPaymentFile(string fileName)
		{
		    // Remove cached entries
		    _memoryCacher.Delete(c.ImportFileDatabaseCachingKey);
            var fileBytes = GetBytesFromDb(fileName);
			if (null == fileBytes)
				throw new ArgumentNullException($"Error. The File \"{fileName}\" does not Exist in the Database");
			var dt = ExcelDataReaderAdapter.ReadExcelFileIntoDataTable(fileBytes.ToArray());
			if (null == dt)
				throw new Exception("Error. Could not Populate the Data Table from the Excel File Bytes " +
									"Returned from the Database");
			var newDt = DataTableProvider.FormatDataTableForPaymentImport(dt, true);
			if (null == newDt)
				throw new Exception("Error. Could not Copy over Contents of the Original Data Table into the " +
									"Cloned Data Table with String Column Types");
			ImportDataTableIntoDb(newDt);
		}

		public IList<ClaimsWithPrescriptionCountsDto> GetClaimsWithPrescriptionCounts(string claimNumber, string firstName,
			string lastName, DateTime? rxDate, string invoiceNumber)
		{
			var paymentSearchResultsDtos = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<ClaimsWithPrescriptionCountsDto>(
				"EXEC [dbo].[uspGetClaimsWithPrescriptionCounts] @ClaimNumber = :ClaimNumber, @FirstName = :FirstName, " +
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
			return paymentSearchResultsDtos ?? new List<ClaimsWithPrescriptionCountsDto>();
		}

		private static IEnumerable<byte> GetBytesFromDb(string fileName) => DisposableService.Using(() 
			=> new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("uspGetFileBytesFromFileName", conn),
					cmd =>
					{
						conn.Open();
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

		private static void ImportDataTableIntoDb(DataTable dt) => DisposableService.Using(() 
			=> new SqlConnection(cs.GetDbConnStr()), conn =>
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