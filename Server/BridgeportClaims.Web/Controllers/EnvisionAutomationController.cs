using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.EnvisionFileProcess;
using BridgeportClaims.Common.Enums;
using s = BridgeportClaims.Common.Constants.StringConstants;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.EmailTemplates;
using Microsoft.AspNet.Identity;
using cs = BridgeportClaims.Common.Config.ConfigService;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/envision")]
    public class EnvisionAutomationController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly IEmailService _emailService;
        private readonly IEnvisionFileProcessor _envisionFileProcessor;
        private readonly IImportFileProvider _importFileProvider;

        public EnvisionAutomationController(IEmailService emailService, IEnvisionFileProcessor envisionFileProcessor, IImportFileProvider importFileProvider)
        {
            _emailService = emailService;
            _envisionFileProcessor = envisionFileProcessor;
            _importFileProvider = importFileProvider;
        }

        [HttpPost]
        [Route("process")]
        public IHttpActionResult ProcessEnvisionFile(int importFileId)
        {
            try
            {
                var userEmail = User.Identity.GetUserName();
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info(
                        $"Starting the Envision file Automation at: {DateTime.UtcNow.ToMountainTime():M/d/yyyy h:mm:ss tt}");
                }
                var (envisionFileName, fullEnvisionFileTemporaryPath) = _envisionFileProcessor.ProcessEnvisionFile(importFileId);
                string msg;
                if (envisionFileName == s.NoEnvisionFilesFound)
                    msg = s.NoEnvisionFilesFound;
                else
                {
                    StartBackgroundThread(envisionFileName, fullEnvisionFileTemporaryPath, userEmail);
                    msg = $"The Envision file import process has been started for \"{envisionFileName}\"." +
                          " It will take a few minutes.... So we'll send you an email when " +
                          "it's done.";
                }
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private void StartBackgroundThread(string envisionFileName, string fullEnvisionFileTemporaryPath, string userEmail)
            => Task.Factory.StartNew(() => ProcessEnvisionImport(envisionFileName, fullEnvisionFileTemporaryPath, userEmail),
                TaskCreationOptions.LongRunning);

        private async Task ProcessEnvisionImport(string envisionFileName, string fullEnvisionFileTemporaryPath, string userEmail)
        {
            try
            {
                const string msg = "The Envision File Import Process Ran Successfully!";
                // Take a third-party CSV reader, and turn that temporarily saved laker file into a Data Table.
                var dataTable = _importFileProvider.RetrieveDataTableFromFullFilePath(fullEnvisionFileTemporaryPath, FileSource.Envision);
                // Import the new file, into the new Staged Envision table that will be imported into the database
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info("About to call ImportEnvisionFile()...");
                }
                // In our case, this call is the big import.
                _importFileProvider.ImportEnvisionFile(dataTable);
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info("Finished inserting Envision records into the database");
                }
                // And finally, mark the file processed.
                _importFileProvider.MarkFileProcessed(envisionFileName);
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info("The file was marked as completed.");
                }
                await _emailService.SendEmail<EmailTemplateProvider>(userEmail, msg, string.Empty,
                    EmailModelEnum.EnvisionImportStatus).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}
