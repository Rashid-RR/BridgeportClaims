using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class SortEpisodeModel
    {
        [Required]
        public int ClaimId { get; set; }
        [Required]
        public string SortColumn { get; set; }
        [Required]
        public string SortDirection { get; set; }
    }
}