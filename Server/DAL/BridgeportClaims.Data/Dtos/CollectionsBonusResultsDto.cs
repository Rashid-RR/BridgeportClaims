﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class CollectionsBonusResultsDto
    {
        [Required]
        public int ClaimId { get; set; }
        [Required]
        public string PatientName { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public decimal BonusAmount { get; set; }
    }
}
