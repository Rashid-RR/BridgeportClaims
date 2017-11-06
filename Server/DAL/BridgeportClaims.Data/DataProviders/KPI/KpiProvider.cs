using System;
using System.Collections.Generic;
using System.Data;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using NHibernate;
using NHibernate.Transform;
using NLog;

namespace BridgeportClaims.Data.DataProviders.KPI
{
    public class KpiProvider : IKpiProvider
    {
        private readonly ISessionFactory _factory;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public KpiProvider(ISessionFactory factory)
        {
            _factory = factory;
        }

        public IList<PaymentTotalsDto> GetPaymentTotalsDtos()
        {
            return DisposableService.Using(() => _factory.OpenSession(), session =>
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
                            Logger.Error(ex);
                            if (tx.IsActive)
                                tx.Rollback();
                            throw;
                        }
                    });
            });
        }
    }
}