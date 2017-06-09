using System;
using System.Data;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using System.Web.Http;
using Ninject.Web.WebApi;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Data.Services.Payors;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.Automappers;
using BridgeportClaims.Business.Config;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.Security;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using BridgeportClaims.Data.SessionFactory;
using NHibernate;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BridgeportClaims.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BridgeportClaims.Web.App_Start.NinjectWebCommon), "Stop")]

namespace BridgeportClaims.Web.App_Start
{
    [System.Runtime.InteropServices.Guid("775DD962-2535-4617-AC22-62CDE8F23DD5")]
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISessionFactory>()
                .ToMethod(c => SessionFactoryBuilder.CreateSessionFactory())
                .InSingletonScope();

            kernel.Bind<ISession>()
                .ToMethod(i => SessionFactoryBuilder.GetSession())
                .InRequestScope()
                .OnActivation(session =>
                {
                    session.BeginTransaction(IsolationLevel.ReadCommitted);
                    session.FlushMode = FlushMode.Commit;
                })
                .OnDeactivation(session =>
                {
                    if (session.Transaction.IsActive)
                    {
                        try
                        {
                            session.Flush();
                            session.Transaction.Commit();
                        }
                        catch
                        {
                            session.Transaction.Rollback();
                            throw;
                        }
                    }
                        session.Transaction.Commit();
                        session.Close();

                    session.Dispose();
                });
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InTransientScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InTransientScope();
            kernel.Bind<ILoggingService>().To<LoggingService>();
            kernel.Bind<IDbccUserOptionsProvider>().To<DbccUserOptionsProvider>();
            kernel.Bind<IConfigService>().To<ConfigService>();
            kernel.Bind<IPayorService>().To<PayorService>();
            kernel.Bind<IStoredProcedureExecutor>().To<StoredProcedureExecutor>();
            kernel.Bind<IPayorMapper>().To<PayorMapper>();
            kernel.Bind<HttpContext>().ToMethod(c => HttpContext.Current);
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            kernel.Bind<IPasswordHasher>().To<PasswordHasher>();

        }        
    }
}
