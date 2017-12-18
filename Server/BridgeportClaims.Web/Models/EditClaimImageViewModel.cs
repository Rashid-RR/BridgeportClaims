﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class EditClaimImageViewModel
    {
        [Required]
        public int DocumentId { get; set; }
        [Required]
        public byte DocumentTypeId { get; set; }
        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
    }
}