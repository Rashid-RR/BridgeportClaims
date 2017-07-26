using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DateDisplay;
using BridgeportClaims.Web.Attributes;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/display")]
    public class DisplayController : BaseApiController
    {
        private readonly IDateDisplayProvider _dateDisplayProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DisplayController(IDateDisplayProvider dateDisplayProvider)
        {
            _dateDisplayProvider = dateDisplayProvider;
        }

        [HttpGet]
        [Route("date")]
        [BridgeportClaimsOutputCache(14400, 14400, false)]
        public async Task<IHttpActionResult> GetDateDisplay()
        {
            try
            {
                return await Task.Run(() => Ok(new {message = _dateDisplayProvider.GetDateDisplay()}));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
