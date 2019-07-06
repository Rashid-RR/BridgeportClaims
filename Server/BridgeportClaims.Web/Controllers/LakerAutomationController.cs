using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using BridgeportClaims.Business.LakerFileProcess;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.EmailTemplates;
using Microsoft.AspNet.Identity;
using cs = BridgeportClaims.Common.Config.ConfigService;


namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/laker")]
    public class LakerAutomationController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly ILakerFileProcessor _lakerFileProcessor;
        private readonly IImportFileProvider _importFileProvider;
        private readonly IEmailService _emailService;

        public LakerAutomationController(
            ILakerFileProcessor lakerFileProcessor, 
            IImportFileProvider importFileProvider, 
            IEmailService emailService)
        {
            _lakerFileProcessor = lakerFileProcessor;
            _importFileProvider = importFileProvider;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("process")]
        public IHttpActionResult ProcessOldestLakerFile()
        {
            try
            {
                var userEmail = User.Identity.GetUserName();
                if (cs.AppIsInDebugMode)
                    Logger.Value.Info(
                        $"Starting the Laker file Automation at: {DateTime.UtcNow.ToMountainTime():M/d/yyyy h:mm:ss tt}");
                var tuple = _lakerFileProcessor.ProcessOldestLakerFile();
                string msg;
                if (tuple.Item1 == StringConstants.NoLakerFilesToImportToast)
                    msg = StringConstants.NoLakerFilesToImportToast;
                else
                {
                    StartBackgroundThread(tuple.Item1, tuple.Item2, userEmail);
                    msg = $"The Laker file import process has been started for \"{tuple.Item1}\"." +
                          " It will take a few minutes.... So we'll send you an email when " +
                          "it's done.";
                }
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private void StartBackgroundThread(string lakerFileName, string fullLakerFileTemporaryPath, string userEmail)
            => Task.Factory.StartNew(() => ProcessLakerImport(lakerFileName, fullLakerFileTemporaryPath, userEmail),
                TaskCreationOptions.LongRunning);
        

        private async Task ProcessLakerImport(string lakerFileName, string fullLakerFileTemporaryPath, string userEmail)
        {
            try
            {
                // Take a third-party CSV reader, and turn that temporarily saved laker file into a Data Table.
                var dataTable = _importFileProvider.RetrieveDataTableFromLatestLakerFile(fullLakerFileTemporaryPath);
                // Import the new file, into the new Staged Laker File that will be imported into the database
                _importFileProvider.LakerImportFileProcedureCall(dataTable);
                // Finally, use the newly imported file, to Upsert the database.
                if (cs.AppIsInDebugMode)
                    Logger.Value.Info("About to call EtlLakerFile()...");
                _importFileProvider.EtlLakerFile(lakerFileName);
                // And finally, mark the file processed.
                _importFileProvider.MarkFileProcessed(lakerFileName);

                if (cs.AppIsInDebugMode)
                    Logger.Value.Info("The file was marked as completed.");
                const string msg = "The Laker File Import Process Ran Successfully!";
                await _emailService.SendEmail<EmailTemplateProvider>(userEmail, msg, string.Empty,
                    EmailModelEnum.LakerImportStatus).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}
