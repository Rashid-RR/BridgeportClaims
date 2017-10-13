using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class UnpaidScriptsViewModel
    {
        [Required]
        public bool IsDefaultSort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(50)]
        [Required]
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