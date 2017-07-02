using NLog;
using System;
using System.Globalization;
using BridgeportClaims.Data.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.StringConstants;
using BridgeportClaims.Data.Mappings.Views;
using cnm = BridgeportClaims.Data.Mappings.ClaimNoteMap;

namespace BridgeportClaims.Data.SessionFactory
{
    public static class SessionFactoryBuilder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                var size = ConfigService.GetAppSetting("AdoNetBatchSize");
                var debug = ConfigService.GetAppSetting("ApplicationIsInDebugMode");
                return new Tuple<int, bool>(Convert.ToInt32(size), Convert.ToBoolean(debug));
            }
        }

        /// <summary>
        /// Initialise singleton instance of ISessionFactory, static 
        /// constructors are only executed once during the
        /// application lifetime - the first time the UnitOfWork class is used
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory()
        {
            try
            {
                var sessionFactory = _sessionFactory ?? (_sessionFactory = Fluently.Configure()
                                         .Database(MsSqlConfiguration)
                                         .Mappings(m =>
                                         {
                                             m.FluentMappings.Add<PatientMap>();
                                             m.FluentMappings.Add<AdjustorMap>();
                                             m.FluentMappings.Add<AspNetRolesMap>();
                                             m.FluentMappings.Add<AspNetUserClaimsMap>();
                                             m.FluentMappings.Add<AspNetUserLoginsMap>();
                                             m.FluentMappings.Add<AspNetUserRolesMap>();
                                             m.FluentMappings.Add<AspNetUsersMap>();
                                             m.FluentMappings.Add<AspNetRolesMap>();
                                             m.FluentMappings.Add<ClaimImageMap>();
                                             m.FluentMappings.Add<ClaimImageTypeMap>();
                                             m.FluentMappings.Add<ClaimMap>();
                                             m.FluentMappings.Add<cnm>();
                                             m.FluentMappings.Add<ClaimNoteTypeMap>();
                                             m.FluentMappings.Add<EpisodeMap>();
                                             m.FluentMappings.Add<EpisodeLinkMap>();
                                             m.FluentMappings.Add<EpisodeLinkTypeMap>();
                                             m.FluentMappings.Add<GenderMap>();
                                             m.FluentMappings.Add<InvoiceMap>();
                                             m.FluentMappings.Add<PharmacyMap>();
                                             m.FluentMappings.Add<PaymentMap>();
                                             m.FluentMappings.Add<PayorMap>();
                                             m.FluentMappings.Add<PrescriptionMap>();
                                             m.FluentMappings.Add<PrescriptionNoteMap>();
                                             m.FluentMappings.Add<PrescriptionNoteTypeMap>();
                                             m.FluentMappings.Add<UsStateMap>();
                                             m.FluentMappings.Add<PrescriptionNoteMappingMap>();
                                             m.FluentMappings.Add<VwPrescriptionNoteMap>();
                                         })
                                         .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                                         .ExposeConfiguration(
                                             config => config.SetProperty("current_session_context_class", "web"))
                                         .ExposeConfiguration(config => config.SetProperty("command_timeout",
                                             TimeSpan.FromMinutes(30).TotalSeconds
                                                 .ToString(CultureInfo.InvariantCulture)))
                                         .Cache(cache => cache.Not.UseSecondLevelCache())
                                         .BuildSessionFactory());
                return sessionFactory;
            }
            catch (FluentConfigurationException fex)
            {
                Logger.Error(fex.InnerException);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private static MsSqlConfiguration MsSqlConfiguration
            => MsSqlConfiguration.MsSql2012.ShowSqlCode().ConnectionString(c =>
                c.FromConnectionStringWithKey(Constants.DbConnStrName))
                .AdoNetBatchSize(AppSettingsItems.Item1);

        private static MsSqlConfiguration ShowSqlCode(this MsSqlConfiguration @this) 
            => AppSettingsItems.Item2 ? @this.ShowSql() : @this;
    }
}