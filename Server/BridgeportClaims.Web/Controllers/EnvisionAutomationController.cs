using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.EnvisionFileProcess;
using s = BridgeportClaims.Common.Constants.StringConstants;
using BridgeportClaims.Common.Extensions;
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

        public EnvisionAutomationController(IEmailService emailService, IEnvisionFileProcessor envisionFileProcessor)
        {
            _emailService = emailService;
            _envisionFileProcessor = envisionFileProcessor;
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
                    Logger.Value.Info($"Starting the Envision file Automation at: {DateTime.UtcNow.ToMountainTime():M/d/yyyy h:mm:ss tt}");
                }
                var tuple = _envisionFileProcessor.ProcessEnvisionFile(importFileId);
                string msg;
                if (tuple.Item1 == s.NoEnvisionFilesFound)
                    msg = s.NoEnvisionFilesFound;
                else
                {
                    StartBackgroundThread(tuple.Item1, tuple.Item2, userEmail);
                    msg = $"The Envision file import process has been started for \"{tuple.Item1}\"." +
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

        private void StartBackgroundThread(string lakerFileName, string fullLakerFileTemporaryPath, string userEmail)
            => Task.Factory.StartNew(() => ProcessLakerImport(/*lakerFileName, fullLakerFileTemporaryPath,*/ userEmail),
                TaskCreationOptions.LongRunning);

        private async Task ProcessLakerImport(string userEmail)
        {
            const string msg = "The Envision File Import Process Ran Successfully!";
            await _emailService.SendEmail<EmailTemplateProvider>(userEmail, msg, string.Empty,
                EmailModelEnum.EnvisionImportStatus).ConfigureAwait(false);
        }
    }
}
