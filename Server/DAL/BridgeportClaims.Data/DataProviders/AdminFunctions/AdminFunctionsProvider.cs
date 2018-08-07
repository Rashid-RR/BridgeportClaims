using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.AdminFunctions
{
	public class AdminFunctionsProvider : IAdminFunctionsProvider
	{
		#region Private Members

		private const string SelectQuery = @"SELECT r.[name] RuleName
											, r.[start_ip_address] StartIpAddress
											, r.[end_ip_address] EndIpAddress
									   FROM sys.firewall_rules AS r
									   WHERE r.[name] != N'AllowAllWindowsAzureIps' AND r.[name] NOT LIKE N'AllowJGurney%'";

		#endregion

		#region Private Methods

		private static void RunSql(string sql) =>
			DisposableService.Using(() => new SqlConnection(cs.GetSecureDbConnStr()), conn =>
			{
				DisposableService.Using(() => new SqlCommand(sql, conn), cmd =>
				{
					cmd.CommandType = CommandType.Text;
					if (conn.State != ConnectionState.Open)
						conn.Open();
					cmd.ExecuteNonQuery();
					if (conn.State != ConnectionState.Closed)
						conn.Close();
				});
			});

		#endregion

		#region Public Methods

		public IEnumerable<InvoiceAmountDto> GetInvoiceAmounts(int claimId, string rxNumber, 
			DateTime? rxDate = null, string invoiceNumber = null) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspGetInvoiceInfo]";
				conn.Open();
				return conn.Query<InvoiceAmountDto>(sp,
					new {ClaimID = claimId, RxNumber = rxNumber, RxDate = rxDate, InvoiceNumber = invoiceNumber},
					commandType: CommandType.StoredProcedure);
			});

		public void DeleteFirewallSetting(string ruleName)
		{
			// Parameterized user-input prevents SQL injection.
			var deleteQuery = $@"DECLARE @name             NVARCHAR(128) = N'{ruleName}'
									   , @RowCount		   INTEGER
									   , @PrntMsg		   NVARCHAR(1000);
								 EXEC [sys].[sp_delete_firewall_rule] @name
								 SET @RowCount = @@ROWCOUNT;
								 IF @RowCount != 1
									 BEGIN
										 SELECT @PrntMsg = N'Error, could not find rule name ''' + @name + ''''
										 RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
										 RETURN;
									 END";
			RunSql(deleteQuery);
		}

		public void AddFirewallSetting(string ruleName, string startIpAddress, string endIpAddress)
		{
			// Parameterized user-input prevents SQL injection.
			var createQuery = $@"DECLARE @name             NVARCHAR(128) = N'{ruleName}'
									   , @start_ip_address VARCHAR(50)   = '{startIpAddress}'
									   , @end_ip_address   VARCHAR(50)   = '{endIpAddress}'
									   , @RowCount		   INTEGER
									   , @PrntMsg		   NVARCHAR(1000);
								 EXEC [sys].[sp_set_firewall_rule] @name = @name
																 , @start_ip_address = @start_ip_address
																 , @end_ip_address = @end_ip_address
								 SET @RowCount = @@ROWCOUNT;
								 IF @RowCount != 1
									 BEGIN
										 SELECT @PrntMsg = N'Error, could not add rule name ''' + @name + ''''
										 RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
										 RETURN;
									 END;";
			RunSql(createQuery);
		}

		public IList<FirewallSetting> GetFirewallSettings() =>
			DisposableService.Using(() => new SqlConnection(cs.GetSecureDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand(SelectQuery, conn), cmd =>
				{
					cmd.CommandType = CommandType.Text;
					if (conn.State != ConnectionState.Open)
						conn.Open();
					return DisposableService.Using(cmd.ExecuteReader, reader =>
					{
						var ruleNameOrdinal = reader.GetOrdinal("RuleName");
						var startIpAddress = reader.GetOrdinal("StartIpAddress");
						var endIpAddress = reader.GetOrdinal("EndIpAddress");
						IList<FirewallSetting> retVal = new List<FirewallSetting>();
						while (reader.Read())
						{
							var result = new FirewallSetting
							{
								RuleName = !reader.IsDBNull(ruleNameOrdinal) ? reader.GetString(ruleNameOrdinal) : string.Empty,
								StartIpAddress = !reader.IsDBNull(startIpAddress) ? reader.GetString(startIpAddress) : string.Empty,
								EndIpAddress = !reader.IsDBNull(endIpAddress) ? reader.GetString(endIpAddress) : string.Empty
							};
							retVal.Add(result);
						}
						return retVal.OrderBy(x => x.RuleName).ToList();
					});
				});
			});

		public void UpdateBilledAmount(int prescriptionId, decimal billedAmount, string modifiedByUserId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspUpdateBilledAmount]";
				conn.Open();
			    conn.Execute(sp,
			        new {PrescriptionID = prescriptionId, BilledAmount = billedAmount,
			            ModifiedByUserID = modifiedByUserId},
			        commandType: CommandType.StoredProcedure);
			});

		#endregion
	}
}