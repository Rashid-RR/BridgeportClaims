using NLog;
using System;
using System.Data;
using System.Runtime.Caching;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Data.DataProviders.DateDisplay
{
    public class DateDisplayProvider : IDateDisplayProvider
    {
        private readonly ISessionFactory _factory;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMemoryCacher _memoryCacher;

        public DateDisplayProvider(ISessionFactory factory, IMemoryCacher memoryCacher)
        {
            _factory = factory;
            _memoryCacher = memoryCacher;
        }

        public string GetDateDisplay()
        {
            var result = _memoryCacher.AddOrGetExisting(c.DateDisplayCacheKey, () =>
            {
                return DisposableService.Using(() => _factory.OpenSession(), session =>
                {
                    return DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
                        transaction =>
                        {
                            try
                            {
                                var retVal = session.CreateSQLQuery("SELECT dtme.udfGetFriendlyDateDisplay()")
                                    .UniqueResult<string>();
                                if (transaction.IsActive)
                                    transaction.Commit();
                                return retVal;
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                                throw;
                            }
                        });
                });
            });
            return result;
        }
    }
}