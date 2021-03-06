﻿using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Business.Models
{
    public class PaymentInputsModel
    {
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