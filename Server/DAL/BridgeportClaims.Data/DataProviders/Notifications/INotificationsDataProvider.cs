using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Notifications
{
    public interface INotificationsDataProvider
    {
        IEnumerable<NotificationDto> GetNotifications();
        void DismissNotification(int notificationId, string dismissedByUserId);
        void DismissEnvisionNotification(int prescriptionId, decimal billedAmount, string modifiedByUserId, int? payorId);
    }
}