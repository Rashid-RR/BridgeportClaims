using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class CreateRoleBindingModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {4} characters long.", MinimumLength = 4)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}