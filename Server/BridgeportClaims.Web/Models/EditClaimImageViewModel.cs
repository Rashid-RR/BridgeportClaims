using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class EditClaimImageViewModel
    {
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public int ClaimId { get; set; }
        [Required]
        public byte DocumentTypeId { get; set; }
        public string RxDate { get; set; }
        public string RxNumber { get; set; }
    }
}