using NLog;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;


namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/laker")]
    public class LakerAutomationController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IImportFileProvider _importFileProvider;

        public LakerAutomationController(IImportFileProvider importFileProvider)
        {
            _importFileProvider = importFileProvider;
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
                    _importFileProvider.ProcessOldestLakerFile();

                    return Ok();
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
