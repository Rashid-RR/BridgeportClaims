namespace BridgeportClaims.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string destinationEmailAddress, EmailTemplate template);
    }
}