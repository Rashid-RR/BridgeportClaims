using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using NHibernate;
using NHibernate.Transform;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.KPI
{
    public class KpiProvider : IKpiProvider
    {
        private readonly Lazy<ISessionFactory> _factory;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public KpiProvider(Lazy<ISessionFactory> factory)
        {
            _factory = factory;
        }

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
                    var r = conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                    if (-1 == r)
                    {
                        throw new Exception($"Error, the stored procedure {sp} did not execute properly.");
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    return false;
                }
            });

        public IList<PaymentTotalsDto> GetPaymentTotalsDtos()
        {
            return DisposableService.Using(() => _factory.Value.OpenSession(), session =>
            {
                return DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
                    tx =>
                    {
                        try
                        {
                            var paymentTotals = session
                                .CreateSQLQuery(@"SELECT DatePosted, TotalPosted FROM rpt.udfGetLastTwentyOneDaysRevenue()")
                                .SetMaxResults(5000)
                                .SetResultTransformer(Transformers.AliasToBean(typeof(PaymentTotalsDto)))
                                .List<PaymentTotalsDto>();
                            if (tx.IsActive)
                                tx.Commit();
                            return paymentTotals;
                        }
                        catch (Exception ex)
                        {
                            Logger.Value.Error(ex);
                            if (tx.IsActive)
                                tx.Rollback();
                            throw;
                        }
                    });
            });
        }
    }
}