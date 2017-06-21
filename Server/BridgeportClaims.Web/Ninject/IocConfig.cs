using System.Data;
using System.Web;
using System.Web.Http;
using BridgeportClaims.Business.Security;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.Automappers;
using BridgeportClaims.Services.Caching;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Services.Constants;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using Microsoft.Owin.Security.DataProtection;
using NHibernate;
using Ninject;
using Ninject.Web.Common;

namespace BridgeportClaims.Web.Ninject
{
    public class IocConfig
    {
        public static void RegisterIoc(HttpConfiguration config)
        {
            var kernel = new StandardKernel(); // Ninject IoC
            //kernel.Load(Assembly.GetExecutingAssembly()); //only required for asp.net mvc (not for webapi)
            // These registrations are "per instance request".
            // See http://blog.bobcravens.com/2010/03/ninject-life-cycle-management-or-scoping/

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
                    try
                    {
                        if (session.Transaction.IsActive)
                        {
                            session.Flush();
                            session.Transaction.Commit();
                        }
                    }
                    catch
                    {
                        if (session.Transaction.IsActive)
                            session.Transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        session.Close();
                        session.Dispose();
                    }
                });
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InTransientScope();
            kernel.Bind<IDbccUserOptionsProvider>().To<DbccUserOptionsProvider>();
            kernel.Bind<IConfigService>().To<ConfigService>();
            kernel.Bind<IPayorService>().To<PayorService>();
            kernel.Bind<IStoredProcedureExecutor>().To<StoredProcedureExecutor>();
            kernel.Bind<IPayorMapper>().To<PayorMapper>();
            kernel.Bind<HttpContext>().ToMethod(c => HttpContext.Current);
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current))
                .InTransientScope();
            kernel.Bind<IPasswordHasher>().To<PasswordHasher>();
            kernel.Bind<IGetClaimsDataProvider>().To<GetClaimsDataProvider>();
            kernel.Bind<IEncryptor>().To<SymmetricEncryptor>();
            kernel.Bind<ICacheService>().To<MemoryCacheService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<IEmailModelGenerator>().To<EmailModelGenerator>();
            kernel.Bind<IConstantsService>().To<ConstantsService>();
            kernel.Bind<IDataProtectionProvider>().ToConstant(Startup.DataProtectionProvider);
                
            // Tell WebApi how to use our Ninject IoC
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}