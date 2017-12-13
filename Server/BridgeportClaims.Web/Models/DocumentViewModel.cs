using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class DocumentViewModel
    {
        [Required]
        public bool IsIndexed { get; set; }
        public DateTime? Date { get; set; }
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