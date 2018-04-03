using NLog;
using System;
using System.Data;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Data.DataProviders.DateDisplay
{
    public class DateDisplayProvider : IDateDisplayProvider
    {
        private readonly Lazy<ISessionFactory> _factory;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IMemoryCacher> _memoryCacher;

        public DateDisplayProvider(Lazy<ISessionFactory> factory)
        {
            _factory = factory;
            _memoryCacher = new Lazy<IMemoryCacher>(() => MemoryCacher.Instance);
        }

        public string GetDateDisplay()
        {
            var result = _memoryCacher.Value.AddOrGetExisting(c.DateDisplayCacheKey, () =>
            {
                return DisposableService.Using(() => _factory.Value.OpenSession(), session =>
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
                                Logger.Value.Error(ex);
                                throw;
                            }
                        });
                });
            });
            return result;
        }
    }
}