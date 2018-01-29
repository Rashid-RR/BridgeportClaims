using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class EpisodesViewModel
    {
        [Required]
        public bool Resolved { get; set; }
        [Required]
        public string SortColumn { get; set; }
        [Required]
        public string SortDirection { get; set; }
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}