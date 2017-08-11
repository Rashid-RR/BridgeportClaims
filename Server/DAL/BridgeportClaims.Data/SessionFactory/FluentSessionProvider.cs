using System;
using System.Data;
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
using System.Web;

namespace BridgeportClaims.Data.SessionFactory
{
	public class FluentSessionProvider : IHttpModule
	{
		[ThreadStatic]
		private static ISessionFactory _sessionFactory;

		private static readonly object Semiphore = new object();

		static FluentSessionProvider()
		{

		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += BeginRequest;
			context.EndRequest += EndRequest;
		}

		public void Dispose()
		{
		}

		private static ISessionFactory SessionFactory
		{
			get
			{
				lock (Semiphore)
				{
					if (null == _sessionFactory)
						_sessionFactory = CreateSessionFactory();
					return _sessionFactory;
				}
			}
		}

		public static ISession GetSession()
		{
			var factory = CreateSessionFactory();
			if (!CurrentSessionContext.HasBind(factory))
				CurrentSessionContext.Bind(factory.OpenSession());
			return factory.GetCurrentSession();
		}

		private static void BeginRequest(object sender, EventArgs e)
		{
			ISession session = SessionFactory.OpenSession();
			session.BeginTransaction(IsolationLevel.ReadUncommitted);
			CurrentSessionContext.Bind(session);
		}

		private static void EndRequest(object sender, EventArgs e)
		{
			ISession session = CurrentSessionContext.Unbind(SessionFactory);
			if (session == null) return;
			try
			{
				if (null != session.Transaction && session.Transaction.IsActive)
					session.Transaction.Commit();
			}
			catch
			{
				if (null != session.Transaction && session.Transaction.IsActive)
					session.Transaction.Rollback();
			}
			finally
			{
				session.Close();
				session.Dispose();
			}
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
			var timeout = TimeSpan.FromMinutes(30).TotalSeconds;
			return Fluently.Configure()
				.Database(CreateDbConfig)
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
				    m.FluentMappings.Add<AcctPayableMap>();
					m.FluentMappings.Add<ClaimMap>();
					m.FluentMappings.Add<cnm>();
					m.FluentMappings.Add<ClaimNoteTypeMap>();
					m.FluentMappings.Add<EpisodeMap>();
					m.FluentMappings.Add<EpisodeTypeMap>();
					m.FluentMappings.Add<EpisodeLinkMap>();
					m.FluentMappings.Add<EpisodeLinkTypeMap>();
				    m.FluentMappings.Add<PaymentMap>();
					m.FluentMappings.Add<GenderMap>();
					m.FluentMappings.Add<InvoiceMap>();
					m.FluentMappings.Add<PharmacyMap>();
					m.FluentMappings.Add<ClaimsUserHistoryMap>();
					m.FluentMappings.Add<DiaryMap>();
					m.FluentMappings.Add<DiaryTypeMap>();
				    m.FluentMappings.Add<ImportFileMap>();
					m.FluentMappings.Add<PayorMap>();
					m.FluentMappings.Add<VwClaimInfoMap>();
					m.FluentMappings.Add<PrescriptionMap>();
					m.FluentMappings.Add<PrescriptionNoteMap>();
					m.FluentMappings.Add<PrescriptionNoteTypeMap>();
					m.FluentMappings.Add<UsStateMap>();
					m.FluentMappings.Add<PrescriptionNoteMappingMap>();
					m.FluentMappings.Add<VwPrescriptionNoteMap>();
				})
				.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
				.ExposeConfiguration(c => c.SetProperty("current_session_context_class", "call"))
				.ExposeConfiguration(c => c.SetProperty("command_timeout",
					timeout.ToString(CultureInfo.InvariantCulture)))
				.CurrentSessionContext<WebSessionContext>()
				.Cache(c => c.Not.UseSecondLevelCache())
				.BuildSessionFactory();
		}



		private static MsSqlConfiguration CreateDbConfig()
			=> MsSqlConfiguration.MsSql2012.ConnectionString(c =>
					c.FromConnectionStringWithKey(Constants.DbConnStrName))
				.AdoNetBatchSize(AppSettingsItems.Item1);
	}
}