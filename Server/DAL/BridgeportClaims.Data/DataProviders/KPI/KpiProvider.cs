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