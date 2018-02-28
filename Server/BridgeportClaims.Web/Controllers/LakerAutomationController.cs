using NLog;
using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Threading.Tasks;
using BridgeportClaims.Business.LakerFileProcess;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.EmailTemplates;
using Microsoft.AspNet.Identity;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/laker")]
    public class LakerAutomationController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
        public async Task<IHttpActionResult> ProcessOldestLakerFile()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var userEmail = User.Identity.GetUserName();
                    if (cs.AppIsInDebugMode)
                        Logger.Info(
                            $"Starting the Laker file Automation at: {DateTime.UtcNow.ToMountainTime():M/d/yyyy h:mm:ss tt}");
                    var tuple = _lakerFileProcessor.ProcessOldestLakerFile();
                    string msg;
                    if (tuple.Item1 == c.NoLakerFilesToImportToast)
                        msg = c.NoLakerFilesToImportToast;
                    else
                    {
                        StartBackgroundThread(async delegate
                            {
                                await ProcessLakerImport(tuple.Item1, tuple.Item2, userEmail).ConfigureAwait(false);
                            });
                        msg = $"The Laker file import process has been started for \"{tuple.Item1}\"." +
                              " It will take a few minutes.... So we'll send you an email when " +
                              "it's done.";
                    }
                    return Ok(new {message = msg});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private static void StartBackgroundThread(ThreadStart threadStart)
        {
            if (threadStart == null) return;
            var thread = new Thread(threadStart) {IsBackground = true};
            thread.Start();
        }

        private async Task ProcessLakerImport(string lakerFileName, string fullLakerFileTemporaryPath,
            string userEmail)
        {
            try
            {
                // Take a third-party CSV reader, and turn that temporarily saved laker file into a Data Table.
                var dataTable = _importFileProvider.RetreiveDataTableFromLatestLakerFile(fullLakerFileTemporaryPath);
                // Import the new file, into the new Staged Laker File that will be imported into the database
                _importFileProvider.LakerImportFileProcedureCall(dataTable);
                // Finally, use the newly imported file, to Upsert the database.
                if (cs.AppIsInDebugMode)
                    Logger.Info("About to call EtlLakerFile()...");
                _importFileProvider.EtlLakerFile(lakerFileName);
                // And finally, mark the file processed.
                _importFileProvider.MarkFileProcessed(lakerFileName);

                if (cs.AppIsInDebugMode)
                    Logger.Info("The file was marked as completed.");
                const string msg = "The Laker File Import Process Ran Successfully!";
                await _emailService.SendEmail<EmailTemplateProvider>(userEmail, msg, string.Empty,
                    EmailModelEnum.LakerImportStatus).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
