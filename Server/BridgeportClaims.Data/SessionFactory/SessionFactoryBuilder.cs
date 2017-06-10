﻿using System;
using System.Globalization;
using BridgeportClaims.Business.Config;
using BridgeportClaims.Data.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace BridgeportClaims.Data.SessionFactory
{
    public static class SessionFactoryBuilder
    {
        [ThreadStatic]
        private static ISessionFactory _sessionFactory;

        public static ISession GetSession()
        {
            var factory = CreateSessionFactory();
            if (!CurrentSessionContext.HasBind(factory))
                CurrentSessionContext.Bind(factory.OpenSession());
            return factory.GetCurrentSession();
        }

        private static Tuple<int, bool> AppSettingsItems
        {
            get
            {
                var configService = new ConfigService();
                return new Tuple<int, bool>(Convert.ToInt32(configService.GetConfigItem("AdoNetBatchSize")),
                    configService.ApplicationIsInDebugMode);
            }
        }

        /// <summary>
        /// Initialise singleton instance of ISessionFactory, static 
        /// constructors are only executed once during the
        /// application lifetime - the first time the UnitOfWork class is used
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory() 
            => _sessionFactory ?? (_sessionFactory = Fluently.Configure()
                                  .Database(MsSqlConfiguration)
                                  .Mappings(m =>
                                   {
                                       m.FluentMappings.Add<AdjustorMap>();
                                       m.FluentMappings.Add<ClaimMap>();
                                       m.FluentMappings.Add<PaymentMap>();
                                       m.FluentMappings.Add<PayorMap>();
                                       m.FluentMappings.Add<UsStateMap>();

                                   })
                                  .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                                  .ExposeConfiguration(config => config.SetProperty("current_session_context_class", "web"))
                                  .ExposeConfiguration(config => config.SetProperty("command_timeout",
                                      TimeSpan.FromMinutes(30).TotalSeconds.ToString(CultureInfo.InvariantCulture)))
                                  .Cache(cache => cache.Not.UseSecondLevelCache())
                                  .BuildSessionFactory());

        private static MsSqlConfiguration MsSqlConfiguration
            => MsSqlConfiguration.MsSql2012.ShowSqlCode().ConnectionString(c =>
                c.FromConnectionStringWithKey("BridgeportClaimsConnectionString"))
                .AdoNetBatchSize(AppSettingsItems.Item1);

        private static MsSqlConfiguration ShowSqlCode(this MsSqlConfiguration @this) 
            => AppSettingsItems.Item2 ? @this.ShowSql() : @this;
    }
}