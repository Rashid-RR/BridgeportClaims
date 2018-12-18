using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class ClaimDocumentViewModel
    {
        public int ClaimId { get; set; }
        [Required]
        [StringLength(50)]
        public string Sort { get; set; }
        [Required]
        [StringLength(5)]
        public string SortDirection { get; set; }
        [Required]
        public int Page { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}