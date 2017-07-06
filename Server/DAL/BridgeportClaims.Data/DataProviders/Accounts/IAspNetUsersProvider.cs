namespace BridgeportClaims.Data.DataProviders.Accounts
{
    public interface IAspNetUsersProvider
    {
        void UpdateFirstOrLastName(string userId, string firstName, string lastName);
        void DeactivateUser(string userId);
        void ActivateUser(string userId);
    }
}