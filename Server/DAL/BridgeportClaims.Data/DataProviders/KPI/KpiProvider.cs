using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;
using ic = BridgeportClaims.Common.Constants.IntegerConstants;

namespace BridgeportClaims.Data.DataProviders.KPI
{
    public class KpiProvider : IKpiProvider
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public IList<LeftRightClaimsDto> GetClaimComparisons(int leftClaimId, int rightClaimId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                var ps = new DynamicParameters();
                ps.Add("@LeftSideClaimID", leftClaimId, DbType.Int32);
                ps.Add("@RightSideClaimID", rightClaimId, DbType.Int32);
                var results = conn.Query<LeftRightClaimsDto>("[dbo].[uspGetLeftRightClaims]", ps,
                    commandType: CommandType.StoredProcedure);
                return results?.ToList();
            });

        public bool SaveClaimMerge(int claimId, int duplicateClaimId, string userId, string claimNumber, int patientId,
            DateTime? injuryDate, int? adjustorId, int payorId, int? claimFlex2Id)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                try
                {
                    const string sp = "[dbo].[uspMergeDuplicateClaims]";
                    conn.Open();
                    var ps = new DynamicParameters();
                    ps.Add("@ClaimID", claimId, DbType.Int32);
                    ps.Add("@DuplicateClaimID", duplicateClaimId, DbType.Int32);
                    ps.Add("@UserID", userId, DbType.String);
                    ps.Add("@ClaimNumber", claimNumber, DbType.AnsiString);
                    ps.Add("@PatientID", patientId, DbType.Int32);
                    ps.Add("@DateOfInjury", injuryDate, DbType.Date);
                    ps.Add("@AdjustorID", adjustorId, DbType.Int32);
                    ps.Add("@PayorID", payorId, DbType.Int32);
                    ps.Add("@ClaimFlex2ID", claimFlex2Id, DbType.Int32);
                    conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    return false;
                }
            });

        public IEnumerable<PaymentTotalsDto> GetPaymentTotalsDtos() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string query = "SELECT DatePosted, TotalPosted FROM rpt.udfGetLastTwentyOneDaysRevenue()";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<PaymentTotalsDto>(query, commandType: CommandType.Text);
            });
    }
}