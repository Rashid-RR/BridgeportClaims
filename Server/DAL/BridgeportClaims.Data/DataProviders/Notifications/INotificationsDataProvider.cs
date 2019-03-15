namespace BridgeportClaims.Data.DataProviders.Notifications
{
    public interface INotificationsDataProvider
    {
        void DismissNotification(int notificationId, string dismissedByUserId);
    }
}