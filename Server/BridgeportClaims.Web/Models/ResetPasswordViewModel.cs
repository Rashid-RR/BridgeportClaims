namespace BridgeportClaims.Web.Models
{
    public sealed class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}