using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class PostPaymentsModel
    {
        [Required]
        public IList<int> PrescriptionIds { get; set; }
        [Required]
        public string CheckNumber { get; set; }
        [Required]
        public decimal CheckAmount { get; set; }
        [Required]
        public decimal AmountSelected { get; set; }
        [Required]
        public decimal AmountToPost { get; set; }
    }
}