namespace BridgeportClaims.Business.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string emailAddress, string password);
    }
}