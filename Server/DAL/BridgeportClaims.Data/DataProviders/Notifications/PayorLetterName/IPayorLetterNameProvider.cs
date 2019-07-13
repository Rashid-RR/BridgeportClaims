namespace BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName
{
    public interface IPayorLetterNameProvider
    {
        void SavePayorLetterNameNotification(int notificationId, string modifiedByUserId, string letterName);
    }
}