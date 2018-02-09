using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class EpisodesViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Required]
        public bool Resolved { get; set; }
        public string OwnerId { get; set; }
        public int? EpisodeCategoryId { get; set; }
        public byte? EpisodeTypeId { get; set; }
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