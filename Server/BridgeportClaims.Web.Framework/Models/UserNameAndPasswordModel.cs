namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class UserNameAndPasswordModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Extension { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}