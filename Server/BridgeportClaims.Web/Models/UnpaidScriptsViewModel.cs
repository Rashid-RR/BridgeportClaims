using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class UnpaidScriptsViewModel
    {
        [Required]
        public bool IsDefaultSort { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
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
        public bool IsArchived { get; set; }
        public IEnumerable<int> PayorIds { get; set; }
        public IEnumerable<string> UserIds { get; set; }
    }
}   