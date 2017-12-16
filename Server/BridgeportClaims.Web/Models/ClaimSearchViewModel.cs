using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class ClaimSearchViewModel
    {
        [Required]
        [StringLength(800)]
        public string SearchText { get; set; }
        [Required]
        public bool ExactMatch { get; set; }
        [Required]
        [StringLength(1)]
        public string Delimiter { get; set; }
    }
}