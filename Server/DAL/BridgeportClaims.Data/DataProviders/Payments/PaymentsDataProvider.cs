using System;
using System.Data;
using System.Linq;
using System.Globalization;
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

	    public decimal GetAmountRemaining(IList<int> claimsIds, string checkNumber)
	    {
	        return DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
	        {
	            return DisposableService.Using(() => new SqlCommand("dbo.uspGetAmountRemaining", conn), cmd =>
	            {
	                cmd.CommandType = CommandType.StoredProcedure;
	                cmd.CommandTimeout = 30;
	                var param = new SqlParameter
	                {
	                    ParameterName = "ClaimIDs",
	                    SqlDbType = SqlDbType.Structured,
	                    Direction = ParameterDirection.Input
	                };
                    cmd.Parameters.Add(param);
	                var nextParam = new SqlParameter
	                {
	                    DbType = DbType.String,
	                    SqlDbType = SqlDbType.VarChar,
	                    Direction = ParameterDirection.Input,
	                    ParameterName = "CheckNumber"
	                };
                    cmd.Parameters.Add(nextParam);
	                var outputParam = new SqlParameter
	                {
	                    ParameterName = "AmountRemaining",
	                    DbType = DbType.Decimal,
	                    SqlDbType = SqlDbType.Money,
	                    Direction = ParameterDirection.Output
	                };
	                cmd.Parameters.Add(outputParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
	                cmd.ExecuteNonQuery();
	                return decimal.TryParse(outputParam.Value?.ToString(), out decimal d) ? d : new decimal();
	            });
	        });
	    }

		public IList<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(IList<int> claimIds)
		{
			var delimitedClaimIds = string.Join(c.Comma, claimIds);
			var claimIdParam = new SqlParameter {ParameterName = "ClaimIDs", Value = delimitedClaimIds, DbType = DbType.String };
			var paymentSearchResultsDtos = _storedProcedureExecutor
				.ExecuteMultiResultStoredProcedure<ClaimsWithPrescriptionDetailsDto>(
					"EXEC [dbo].[uspGetClaimsWithPrescriptionDetails] @ClaimIDs = :ClaimIDs",
					new List<SqlParameter> {claimIdParam});
			return paymentSearchResultsDtos?.OrderByDescending(x => x.RxDate).ToList();
		}

		public void ImportPaymentFile(string fileName)
		{
			// Remove cached entries
			_memoryCacher.DeleteIfExists(c.ImportFileDatabaseCachingKey);
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
				return DisposableService.Using(() => new SqlCommand("dbo.uspGetFileBytesFromFileName", conn),
					cmd =>
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileName;
						if (conn.State != ConnectionState.Open)
							conn.Open();
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
				DisposableService.Using(() => new SqlCommand("dbo.uspImportPaymentFromDataTable", conn),
					cmd =>
					{
						cmd.CommandType = CommandType.StoredProcedure;
						var dataTableParam = new SqlParameter
						{
							Value = dt,
							SqlDbType = SqlDbType.Structured,
							ParameterName = "@Payment"
						};
						cmd.Parameters.Add(dataTableParam);
						if (conn.State != ConnectionState.Open)
							conn.Open();
						cmd.ExecuteNonQuery();
					});
			});

		public PostPaymentReturnDto PostPayment(IEnumerable<int> prescriptionIds, string checkNumber,
			decimal checkAmount, decimal amountSelected, decimal amountToPost)
		{
			return DisposableService.Using(()
				 => new SqlConnection(cs.GetDbConnStr()), conn =>
			 {
				 return DisposableService.Using(() => new SqlCommand("dbo.uspPostPayment", conn),
					 cmd =>
					 {
						 cmd.CommandType = CommandType.StoredProcedure;
						 var prescriptionIdsParam = new SqlParameter
						 {
							 Value = CreateDataTable(prescriptionIds),
							 SqlDbType = SqlDbType.Structured,
							 ParameterName = "@PrescriptionIDs",
							 Direction = ParameterDirection.Input,
							 TypeName = "dbo.udtPrescriptionID"
						 };
						 var checkNumberParam = new SqlParameter
						 {
							 Value = checkNumber,
							 SqlDbType = SqlDbType.VarChar,
							 Direction = ParameterDirection.Input,
							 ParameterName = "@CheckNumber"
						 };
						 var checkAmountParam = new SqlParameter
						 {
							 Value = checkAmount,
							 SqlDbType = SqlDbType.Money,
							 Direction = ParameterDirection.Input,
							 ParameterName = "@CheckAmount"
						 };
						 var amountSelectedParam = new SqlParameter
						 {
							 Value = amountSelected,
							 SqlDbType = SqlDbType.Money,
							 Direction = ParameterDirection.Input,
							 ParameterName = "@AmountSelected"
						 };
						 var amountToPostParam = new SqlParameter
						 {
							 Value = amountToPost,
							 SqlDbType = SqlDbType.Money,
							 Direction = ParameterDirection.Input,
							 ParameterName = "@AmountToPost"
						 };
						 var amountRemainingParam = cmd.CreateParameter();
						 amountRemainingParam.SqlDbType = SqlDbType.Money;
						 amountRemainingParam.DbType = DbType.Decimal;
						 amountRemainingParam.Precision = 18;
						 amountRemainingParam.Scale = 2;
						 amountRemainingParam.Direction = ParameterDirection.Output;
						 amountRemainingParam.ParameterName = "@AmountRemaining";
						 
						 cmd.Parameters.Add(prescriptionIdsParam);
						 cmd.Parameters.Add(checkNumberParam);
						 cmd.Parameters.Add(checkAmountParam);
						 cmd.Parameters.Add(amountSelectedParam);
						 cmd.Parameters.Add(amountToPostParam);
						 cmd.Parameters.Add(amountRemainingParam);
						 if (conn.State != ConnectionState.Open)
							 conn.Open();
						 var retVal = new PostPaymentReturnDto();
						 DisposableService.Using(cmd.ExecuteReader, reader =>
						 {    
							 var prescriptionIdOrdinal = reader.GetOrdinal("PrescriptionID");
							 var outstandingOrdinal = reader.GetOrdinal("Outstanding");
							 while (reader.Read())
							 {
								 var postPaymentPrescriptionReturnDto =
									 new PostPaymentPrescriptionReturnDto
									 {
										 PrescriptionId = reader.GetInt32(prescriptionIdOrdinal),
										 Outstanding = reader.GetDecimal(outstandingOrdinal)
									 };
								 retVal.PostPaymentPrescriptionReturnDtos.Add(postPaymentPrescriptionReturnDto);
							 }
							 return retVal;
						 });
						 const NumberStyles style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
						 var culture = CultureInfo.CreateSpecificCulture("en-US");
						 retVal.AmountRemaining = decimal.TryParse(amountRemainingParam.Value.ToString(), style, culture, out decimal d)
							 ? d : default(decimal);
						 return retVal;
					 });

			 });
		}

		private static DataTable CreateDataTable(IEnumerable<int> ids)
		{
			var table = new DataTable();
			table.Columns.Add("PrescriptionID", typeof(int));
			foreach (var id in ids)
				table.Rows.Add(id);
			return table;
		}
	}
}