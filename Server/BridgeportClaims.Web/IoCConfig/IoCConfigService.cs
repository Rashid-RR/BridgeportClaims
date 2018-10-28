using Autofac;
using System.Web;
using System.Reflection;
using Autofac.Integration.WebApi;
using BridgeportClaims.Business.BillingStatement;
using BridgeportClaims.Business.LakerFileProcess;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Business.PrescriptionReports;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.DataProviders.Accounts;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.DataProviders.UserRoles;
using BridgeportClaims.Data.DataProviders.ClaimNotes;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Data.DataProviders.PrescriptionNotes;
using BridgeportClaims.Data.DataProviders.ClaimsUserHistories;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Web.EmailTemplates;
using BridgeportClaims.CsvReader.CsvReaders;
using BridgeportClaims.Data.DataProviders.AdjustorSearches;
using BridgeportClaims.Data.DataProviders.AdminFunctions;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Data.DataProviders.ClaimsEdit;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
using BridgeportClaims.Data.DataProviders.Clients;
using BridgeportClaims.Data.DataProviders.Dashboards;
using BridgeportClaims.Data.DataProviders.Diaries;
using BridgeportClaims.Data.DataProviders.DocumentIndexes;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Data.DataProviders.EpisodeNotes;
using BridgeportClaims.Data.DataProviders.KPI;
using BridgeportClaims.Data.DataProviders.LetterGenerations;
using BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName;
using BridgeportClaims.Data.DataProviders.PayorSearches;
using BridgeportClaims.Data.DataProviders.PrescriptionPayments;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.DataProviders.Reports;
using BridgeportClaims.Data.DataProviders.Users;
using BridgeportClaims.Data.DataProviders.Utilities;
using BridgeportClaims.Pdf.Factories;
using BridgeportClaims.RedisCache.Clearing;
using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.Word.FileDriver;
using BridgeportClaims.Word.Templating;
using BridgeportClaims.Word.WordProvider;
using Microsoft.AspNet.SignalR;
using NLog;

namespace BridgeportClaims.Web.IoCConfig
{
    public class IoCConfigService
    {
        public static ContainerBuilder Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PayorsDataProvider>().As<IPayorsDataProvider>().InstancePerRequest();
            builder.RegisterType<ClaimsDataProvider>().As<IClaimsDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();
            builder.RegisterType<PrescriptionNoteTypesDataProvider>().As<IPrescriptionNoteTypesDataProvider>().InstancePerRequest();
            builder.RegisterType<EmailModelGenerator>().As<IEmailModelGenerator>().InstancePerRequest();
            builder.RegisterType<EpisodesDataProvider>().As<IEpisodesDataProvider>().InstancePerRequest();
            builder.RegisterType<LakerFileProcessor>().As<ILakerFileProcessor>().InstancePerRequest();
            builder.RegisterType<AspNetUsersProvider>().As<IAspNetUsersProvider>().InstancePerRequest();
            builder.RegisterType<DiaryProvider>().As<IDiaryProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionsDataProvider>().As<IPrescriptionsDataProvider>().InstancePerRequest();
            builder.RegisterType<CsvReaderProvider>().As<ICsvReaderProvider>().InstancePerRequest();
            builder.RegisterType<ImportFileProvider>().As<IImportFileProvider>().InstancePerRequest();
            builder.RegisterType<ReportsDataProvider>().As<IReportsDataProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionPaymentProvider>().As<IPrescriptionPaymentProvider>().InstancePerRequest();
            builder.RegisterType<CsvToolsProvider>().As<ICsvToolsProvider>().InstancePerRequest();
            builder.RegisterType<DocumentDataProvider>().As<IDocumentDataProvider>().InstancePerRequest();
            builder.RegisterType<DocumentIndexProvider>().As<IDocumentIndexProvider>().InstancePerRequest();
            builder.RegisterType<ClaimSearchProvider>().As<IClaimSearchProvider>().InstancePerRequest();
            builder.RegisterType<CachingClearingService>().As<ICachingClearingService>().InstancePerRequest();
            builder.RegisterType<ClaimImageProvider>().As<IClaimImageProvider>().InstancePerRequest();
            builder.RegisterType<ClaimsEditProvider>().As<IClaimsEditProvider>().InstancePerRequest();
            builder.RegisterType<PayorSearchProvider>().As<IPayorSearchProvider>().InstancePerRequest();
            builder.RegisterType<RedisDomain>().As<IRedisDomain>().InstancePerRequest();
            builder.RegisterType<AdjustorSearchProvider>().As<IAdjustorSearchProvider>().InstancePerRequest();
            builder.RegisterType<DashboardProvider>().As<IDashboardProvider>().InstancePerRequest();
            builder.RegisterType<EpisodeNoteProvider>().As<IEpisodeNoteProvider>().InstancePerRequest();
            builder.RegisterType<AdminFunctionsProvider>().As<IAdminFunctionsProvider>().InstancePerRequest();
            builder.RegisterType<UsersProvider>().As<IUsersProvider>().InstancePerRequest();
            builder.RegisterType<PdfFactory>().As<IPdfFactory>().InstancePerRequest();
            builder.RegisterType<PrescriptionReportFactory>().As<IPrescriptionReportFactory>().InstancePerRequest();
            builder.RegisterType<WordDocumentProvider>().As<IWordDocumentProvider>().InstancePerRequest();
            builder.RegisterType<WordFileDriver>().As<IWordFileDriver>().InstancePerRequest();
            builder.RegisterType<LetterGenerationProvider>().As<ILetterGenerationProvider>().InstancePerRequest();
            builder.RegisterType<ClientDataProvider>().As<IClientDataProvider>().InstancePerRequest();
            builder.RegisterType<WordTemplater>().As<IWordTemplater>().InstancePerRequest();
            builder.RegisterType<PayorLetterNameProvider>().As<IPayorLetterNameProvider>().InstancePerRequest();
            builder.RegisterType<BillingStatementProvider>().As<IBillingStatementProvider>().InstancePerRequest();

            // SignalR
            var hubConfig = new HubConfiguration();
            builder.RegisterInstance(hubConfig);
            
            builder.RegisterType<ClaimsUserHistoryProvider>().As<IClaimsUserHistoryProvider>().InstancePerRequest();
            builder.RegisterType<AssignUsersToRolesProvider>().As<IAssignUsersToRolesProvider>().InstancePerRequest();
            builder.RegisterType<ClaimNotesDataProvider>().As<IClaimNotesDataProvider>().InstancePerRequest();
            builder.RegisterType<PaymentsBusiness>().As<IPaymentsBusiness>().InstancePerRequest();
            builder.RegisterType<PaymentsDataProvider>().As<IPaymentsDataProvider>().InstancePerRequest();
            builder.RegisterType<UtilitiesProvider>().As<IUtilitiesProvider>().InstancePerRequest();
            builder.RegisterType<KpiProvider>().As<IKpiProvider>().InstancePerRequest();
            builder.RegisterType<EmailTemplateProvider>().As<IEmailTemplateProvider>().InstancePerRequest();
            builder.RegisterType<PrescriptionNotesDataProvider>().As<IPrescriptionNotesDataProvider>().InstancePerRequest();
            builder.Register(ctx => LogManager.GetCurrentClassLogger()).As<ILogger>().SingleInstance();

            // Singletons
            builder.RegisterType<MemoryCacher>().As<IMemoryCacher>().SingleInstance();
            builder.Register(c => HttpContext.Current).As<HttpContext>().InstancePerRequest();
            builder.Register(c => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>()
                .InstancePerRequest();
            return builder;
        }
    }
}