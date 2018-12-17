using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class SmallDocumentViewModel
    {
        public string Date { get; set; }
        public string FileName { get; set; }
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