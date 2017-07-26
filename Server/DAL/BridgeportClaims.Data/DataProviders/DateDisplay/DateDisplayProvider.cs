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
            var result = _memoryCacher.GetValue(c.DateDisplayCacheKey) as string;
            if (null != result)
                return result;
            DisposableService.Using(() => _factory.OpenSession(), session =>
            {
                DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
                    transaction =>
                    {
                        try
                        {
                            result = session.CreateSQLQuery("SELECT dtme.udfGetFriendlyDateDisplay()")
                                .UniqueResult<string>();
                            if (transaction.IsActive)
                                transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                            throw;
                        }
                    });
            });
            _memoryCacher.Add(c.DateDisplayCacheKey, result, DateTimeOffset.UtcNow.AddHours(4));
            return result;
        }
    }
}