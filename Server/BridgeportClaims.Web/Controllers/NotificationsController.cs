using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Notifications;
using BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/notifications")]
    [Authorize(Roles = "User")]
    public class NotificationsController : BaseApiController
    {
        private readonly Lazy<IPayorLetterNameProvider> _payorLetterNameProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<INotificationsDataProvider> _notificationsDataProvider;

        public NotificationsController(Lazy<IPayorLetterNameProvider> payorLetterNameProvider,
            Lazy<INotificationsDataProvider> notificationsDataProvider)
        {
            _payorLetterNameProvider = payorLetterNameProvider;
            _notificationsDataProvider = notificationsDataProvider;
        }

        [HttpPost]
        [Route("dismiss")]
        public IHttpActionResult Dismiss(int notificationId)
        {
            try
            {
                var dismissedByUserId = User.Identity.GetUserId();
                _notificationsDataProvider.Value.DismissNotification(notificationId, dismissedByUserId);
                return Ok(new { message = "The notification was dismissed successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("save-payor-letter-name")]
        public IHttpActionResult SavePayorLetterNames(SavePayorLetterNameModel model)
        {
            try
            {
                if (model.LetterName.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(model.LetterName));
                if (model.NotificationId == default(int))
                    throw new Exception($"Error, the notification Id {model.NotificationId} doesn't exist.");
                var userId = User.Identity.GetUserId();
                _payorLetterNameProvider.Value.SavePayorLetterNameNotification(model.NotificationId, userId,
                    model.LetterName);
                return Ok(new {message = $"The letter name {model.LetterName} was saved successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetNotifications()
        {
            try
            {
                var notifications = _payorLetterNameProvider.Value.GetNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
