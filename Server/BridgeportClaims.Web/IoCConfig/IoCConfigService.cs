using System.Data;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.DataProviders.PrescriptionNotes;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.Automappers;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using NHibernate;

namespace BridgeportClaims.Web.IoCConfig
{
    public class IoCConfigService
    {
        public static ContainerBuilder Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DbccUserOptionsProvider>().As<IDbccUserOptionsProvider>().InstancePerRequest();
            builder.RegisterType<PayorsDataProvider>().As<IPayorsDataProvider>().InstancePerRequest();
            builder.RegisterType<StoredProcedureExecutor>().As<IStoredProcedureExecutor>().InstancePerRequest();
            builder.RegisterType<PayorMapper>().As<IPayorMapper>().InstancePerRequest();
            builder.RegisterType<GetClaimsDataProvider>().As<IGetClaimsDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();
            builder.RegisterType<PrescriptionNoteTypesDataProvider>().As<IPrescriptionNoteTypesDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailModelGenerator>().As<IEmailModelGenerator>().InstancePerRequest();
            builder.RegisterType<AssignUsersToRolesProvider>().As<IAssignUsersToRolesProvider>().InstancePerRequest();
            builder.RegisterType<ClaimNotesDataProvider>().As<IClaimNotesDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailTemplateProvider>().As<IEmailTemplateProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionNotesDataProvider>().As<IPrescriptionNotesDataProvider>().InstancePerRequest();
            builder.Register(c => SessionFactoryBuilder.CreateSessionFactory()).As<ISessionFactory>().SingleInstance();
            builder.Register(c => SessionFactoryBuilder.GetSession()).As<ISession>().OnActivated(session =>
            {
                session.Instance.BeginTransaction(IsolationLevel.ReadCommitted);
                session.Instance.FlushMode = FlushMode.Commit;
            }).OnRelease(session =>
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
            }).InstancePerRequest();
            builder.Register(c => HttpContext.Current).As<HttpContext>().InstancePerRequest();
            builder.Register(c => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>()
                .InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            return builder;
        }
    }
}