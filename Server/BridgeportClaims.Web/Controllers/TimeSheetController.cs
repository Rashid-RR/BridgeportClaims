using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.TimeSheets;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/timesheet")]
    public class TimeSheetController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<ITimeSheetProvider> _timeSheetProvider;

        public TimeSheetController(Lazy<ITimeSheetProvider> timeSheetProvider)
        {
            _timeSheetProvider = timeSheetProvider;
        }

        [HttpPost]
        [Route("clock-in")]
        public IHttpActionResult ClockIn()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _timeSheetProvider.Value.ClockIn(userId);
                return Ok(new {message = "Clocked In Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("clock-out")]
        public IHttpActionResult ClockOut()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _timeSheetProvider.Value.ClockOut(userId);
                return Ok(new { message = "Clocked Out Successfully" });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-start-time")]
        public IHttpActionResult GetStartTime()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var startTime = _timeSheetProvider.Value.GetStartTime(userId);
                return Ok(startTime);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
