using BridgeportClaims.Business.Config;
using BridgeportClaims.Business.Logging;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BridgeportClaims.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BridgeportClaims.Web.App_Start.NinjectWebCommon), "Stop")]

namespace BridgeportClaims.Web.App_Start
{
    using System;
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
    using NHibernate;
    using BridgeportClaims.Data.NHibernateProviders;

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
            kernel.Bind<ISession>()
                .ToMethod(i => FluentSessionProvider.GetCurrentSession())
                .InRequestScope()
                .OnDeactivation((context, session) =>
                {
                    if (session.Transaction.IsActive)
                        session.Transaction.Commit();

                    if (session.IsOpen)
                        session.Close();

                    session.Dispose();
                });
            kernel.Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));
            kernel.Bind<ILoggingService>().To<LoggingService>();
            kernel.Bind<IDbccUserOptionsProvider>().To<DbccUserOptionsProvider>();
            kernel.Bind<IConfigService>().To<ConfigService>();
            kernel.Bind<IPayorService>().To<PayorService>();
            kernel.Bind<IStoredProcedureExecutor>().To<StoredProcedureExecutor>();
            kernel.Bind<HttpContext>().ToMethod(c => HttpContext.Current);
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
        }        
    }
}
