using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName
{
    public interface IPayorLetterNameProvider
    {
        IList<NotificationDto> GetNotifications();
        void SavePayorLetterNameNotification(int notificationId, string modifiedByUserId, string letterName);
    }
}