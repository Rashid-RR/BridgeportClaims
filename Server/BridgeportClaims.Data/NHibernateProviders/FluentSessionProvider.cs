using System;
using System.Data;
using System.Globalization;
using System.Web;
using NHibernate;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace BridgeportClaims.Data.NHibernateProviders
{
    public class FluentSessionProvider : IHttpModule
    {
        private static ISessionFactory _sessionFactory;
        private static readonly object Semiphore = new object();

        public static ISessionFactory SessionFactory
        {
            get
            {
                lock (Semiphore)
                {
                    return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
                }
            }
        }

        static FluentSessionProvider() { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        public void Dispose() { }

        public static ISession GetCurrentSession()
        {
            var factory = SessionFactory;
            if (!CurrentSessionContext.HasBind(factory))
                CurrentSessionContext.Bind(factory.OpenSession());
            return factory.GetCurrentSession();
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            var session = SessionFactory.OpenSession();
            session.BeginTransaction(IsolationLevel.Snapshot);
            CurrentSessionContext.Bind(session);
        }

        private static void EndRequest(object sender, EventArgs e)
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);
            if (session == null) return;
            try
            {
                session.Transaction.Commit();
            }
            catch
            {
                session.Transaction.Rollback();
            }
            finally
            {
                session.Close();
                session.Dispose();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var timeout = TimeSpan.FromMinutes(30).TotalSeconds;
            return Fluently.Configure()
                .Database(CreateDbConfig)
                .Mappings(m =>
                {
                    //m.FluentMappings.Add<OrdersMap>();
                })
                .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "call"))
                .ExposeConfiguration(c => c.SetProperty("command_timeout", timeout.ToString(CultureInfo.InvariantCulture)))
                .CurrentSessionContext<WebSessionContext>()
                .Cache(c => c.Not.UseSecondLevelCache())
                .BuildSessionFactory();
        }

        private static MsSqlConfiguration CreateDbConfig()
        {
            return MsSqlConfiguration
                .MsSql2012 // Actually 2014. Currently no dialect for MSSQL 2014 exists yet.
                // .ShowSql() // Nah... Performance Impact.
                .ConnectionString(c =>
                    c.FromConnectionStringWithKey(
                        "JGPortal.Services.Properties.Settings.JGPortalConnectionString"));
        }
    }
}