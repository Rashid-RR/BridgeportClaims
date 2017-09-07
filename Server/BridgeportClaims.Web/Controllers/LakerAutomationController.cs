using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using BridgeportClaims.Business.LakerFileProcess;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/laker")]
    public class LakerAutomationController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ILakerFileProcessor _lakerFileProcessor;

        public LakerAutomationController(ILakerFileProcessor lakerFileProcessor)
        {
            _lakerFileProcessor = lakerFileProcessor;
        }

        [HttpPost]
        [Route("process")]
        public async Task<IHttpActionResult> ProcessOldestLakerFile()
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Info($"Starting the Laker file Automation at: {DateTime.UtcNow.ToLocalTime():M/d/yyyy h:mm:ss tt}");
                    var fileName = _lakerFileProcessor.ProcessOldestLakerFile();
                    string msg;
                    if (fileName == c.NoLakerFilesToImportToast)
                        msg = c.NoLakerFilesToImportToast;
                    else
                        msg = $"The Laker file import process has been started for \"{fileName}\"." +
                              $" It will take a few minutes.... So we'll send you an email when " +
                              $"it's done.";
                    return Ok(new { message = msg});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
