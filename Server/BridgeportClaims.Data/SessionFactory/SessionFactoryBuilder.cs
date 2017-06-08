using System;
using System.Globalization;
using BridgeportClaims.Data.Mappings;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace BridgeportClaims.Data.SessionFactory
{
    public static class SessionFactoryBuilder
    {
        /// <summary>
        /// Initialise singleton instance of ISessionFactory, static 
        /// constructors are only executed once during the
        /// application lifetime - the first time the UnitOfWork class is used
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory()
        =>
            Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.
                        ConnectionString(c =>
                            c.FromConnectionStringWithKey("BridgeportClaimsConnectionString"))
                                .AdoNetBatchSize(100))
                .Mappings(x => 
                        x.AutoMappings.
                            Add(AutoMap.AssemblyOf<PayorMap>(new AutomappingConfiguration())
                    // Noth using Overrides yet..
                    //.UseOverridesFromAssemblyOf<Payor>()
                ))
                .ExposeConfiguration(config => config.SetProperty("current_session_context_class", "call"))
                .ExposeConfiguration(config => config.SetProperty("command_timeout",
                    TimeSpan.FromMinutes(30).TotalSeconds.ToString(CultureInfo.InvariantCulture)))
                .Cache(cache => cache.Not.UseSecondLevelCache())
                .BuildSessionFactory(); // Wow, FINALLY return an ISessionFactory object.
    }
}