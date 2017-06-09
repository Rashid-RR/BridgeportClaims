using System;
using System.Globalization;
using BridgeportClaims.Business.Config;
using BridgeportClaims.Data.Mappings;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;

namespace BridgeportClaims.Data.SessionFactory
{
    public static class SessionFactoryBuilder
    {
        private static ISessionFactory _sessionFactory;

        public static ISession GetSession()
        {
            var factory = CreateSessionFactory();
            if (!CurrentSessionContext.HasBind(factory))
                CurrentSessionContext.Bind(factory.OpenSession());
            return factory.GetCurrentSession();
        }

        /// <summary>
        /// Initialise singleton instance of ISessionFactory, static 
        /// constructors are only executed once during the
        /// application lifetime - the first time the UnitOfWork class is used
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory()
        {
            return _sessionFactory ?? (_sessionFactory = Fluently.Configure()
                       .Database(CreateMsSqlConfiguration())
                       .Mappings(m =>
                       {
                           m.FluentMappings.Add<AdjustorMap>();
                           m.FluentMappings.Add<ClaimMap>();
                           m.FluentMappings.Add<PaymentMap>();
                           m.FluentMappings.Add<PayorMap>();
                           m.FluentMappings.Add<UsStateMap>();
                       })
                        .ExposeConfiguration(config => config.SetProperty("current_session_context_class", "call"))
                       .ExposeConfiguration(config => config.SetProperty("command_timeout",
                           TimeSpan.FromMinutes(30).TotalSeconds.ToString(CultureInfo.InvariantCulture)))
                       .Cache(cache => cache.Not.UseSecondLevelCache())
                       .BuildSessionFactory());
        }

        private static MsSqlConfiguration CreateMsSqlConfiguration()
        {
            return MsSqlConfiguration
                .MsSql2012 // Actually Azure SQL Server 2016. Currently no dialect for MSSQL 2016 exists yet.
                .ShowSqlCode() // Nah... Performance Impact.
                .ConnectionString(c =>
                c.FromConnectionStringWithKey(
                "BridgeportClaimsConnectionString"))
                .AdoNetBatchSize(100);
        }

        private static MsSqlConfiguration ShowSqlCode(this MsSqlConfiguration @this)
        {
            var configService = new ConfigService();
            return configService.ApplicationIsInDebugMode ? @this.ShowSql() : @this;
        }
    }
}