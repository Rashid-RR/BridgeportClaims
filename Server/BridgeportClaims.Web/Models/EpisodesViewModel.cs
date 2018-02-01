using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class EpisodesViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public bool Resolved { get; set; }
        public string OwnerId { get; set; }
        public int? EpisodeCategoryId { get; set; }
        public int? EpisodeTypeId { get; set; }
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