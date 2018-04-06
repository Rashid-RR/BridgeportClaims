using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/notifications")]
    [Authorize(Roles = "User")]
    public class NotificationsController : BaseApiController
    {
        private readonly IPayorLetterNameProvider _payorLetterNameProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public NotificationsController(IPayorLetterNameProvider payorLetterNameProvider)
        {
            _payorLetterNameProvider = payorLetterNameProvider;
        }

        [HttpPost]
        [Route("save-payor-letter-name")]
        public async Task<IHttpActionResult> SavePayorLetterNames(SavePayorLetterNameModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (model.LetterName.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(model.LetterName));
                    if (model.NotificationId == default(int))
                        throw new Exception($"Error, the notification Id {model.NotificationId} doesn't exist.");
                    var userId = User.Identity.GetUserId();
                    _payorLetterNameProvider.SavePayorLetterNameNotification(model.NotificationId, userId,
                        model.LetterName);
                    return Ok(new {message = $"The letter name {model.LetterName} was saved successfully."});

                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get")]
        public async Task<IHttpActionResult> GetNotifications()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var notifications = _payorLetterNameProvider.GetNotifications();
                    return Ok(notifications);

                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
