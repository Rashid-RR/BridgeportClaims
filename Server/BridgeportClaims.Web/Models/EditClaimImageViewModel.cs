using System;
using System.ComponentModel.DataAnnotations;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class EditClaimImageViewModel
    {
        private string _type;

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public string Type
        {
            get => _type.IsNotNullOrWhiteSpace() ? _type.ToLower() : null;
            set => _type = value;
        }

        public string TypeNormal => _type;

        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
    }
}