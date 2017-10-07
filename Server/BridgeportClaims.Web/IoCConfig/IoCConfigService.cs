﻿using Autofac;
using NHibernate;
using System.Web;
using System.Data;
using System.Reflection;
using Autofac.Integration.WebApi;
using BridgeportClaims.Business.LakerFileProcess;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory;
using BridgeportClaims.Entities.Automappers;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.DataProviders.Accounts;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.DataProviders.UserRoles;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Data.DataProviders.UserOptions;
using BridgeportClaims.Data.DataProviders.DateDisplay;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Data.DataProviders.PrescriptionNotes;
using BridgeportClaims.Data.DataProviders.ClaimsUserHistories;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Web.EmailTemplates;
using BridgeportClaims.CsvReader.CsvReaders;
using BridgeportClaims.Data.DataProviders.PrescriptionPayments;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.DataProviders.Utilities;

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
            builder.RegisterType<ClaimsDataProvider>().As<IClaimsDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();
            builder.RegisterType<PrescriptionNoteTypesDataProvider>().As<IPrescriptionNoteTypesDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailModelGenerator>().As<IEmailModelGenerator>().InstancePerRequest();
            builder.RegisterType<EpisodesDataProvider>().As<IEpisodesDataProvider>().InstancePerRequest();
            builder.RegisterType<LakerFileProcessor>().As<ILakerFileProcessor>().InstancePerRequest();
            builder.RegisterType<AspNetUsersProvider>().As<IAspNetUsersProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionsProvider>().As<IPrescriptionsProvider>().InstancePerRequest();
            builder.RegisterType<CsvReaderProvider>().As<ICsvReaderProvider>().InstancePerRequest();
            builder.RegisterType<ImportFileProvider>().As<IImportFileProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionPaymentProvider>().As<IPrescriptionPaymentProvider>().InstancePerRequest();
            
            builder.RegisterType<ClaimsUserHistoryProvider>().As<IClaimsUserHistoryProvider>().InstancePerRequest();
            builder.RegisterType<AssignUsersToRolesProvider>().As<IAssignUsersToRolesProvider>().InstancePerRequest();
            builder.RegisterType<ClaimNotesDataProvider>().As<IClaimNotesDataProvider>().InstancePerRequest();
            builder.RegisterType<PaymentsBusiness>().As<IPaymentsBusiness>().InstancePerRequest();
            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerRequest();
            builder.RegisterType<UtilitiesProvider>().As<IUtilitiesProvider>().InstancePerRequest();
            builder.RegisterType<EmailTemplateProvider>().As<IEmailTemplateProvider>().InstancePerRequest();
            builder.RegisterType<DateDisplayProvider>().As<IDateDisplayProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionNotesDataProvider>().As<IPrescriptionNotesDataProvider>().InstancePerRequest();
            // Singletons

            builder.RegisterType<MemoryCacher>().As<IMemoryCacher>().SingleInstance();
            builder.Register(c => FluentSessionProvider.CreateSessionFactory()).As<ISessionFactory>().SingleInstance();
            builder.Register(c => FluentSessionProvider.GetSession()).As<ISession>().OnActivated(session =>
            {
                session.Instance.BeginTransaction(IsolationLevel.ReadCommitted);
                session.Instance.FlushMode = FlushMode.Commit;
            }).OnRelease(session =>
            {
                try
                {
                    if (session.Transaction.IsActive)
                        session.Transaction.Commit();
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