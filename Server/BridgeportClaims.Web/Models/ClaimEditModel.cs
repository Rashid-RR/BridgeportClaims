using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    public sealed class ClaimEditModel
    {
        [Required]
        public int ClaimId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int PayorId { get; set; }
        public int? AdjustorId { get; set; }
        public string AdjustorPhone { get; set; }
        public DateTime? DateOfInjury { get; set; }
        public string AdjustorFax { get; set; }
    }
}