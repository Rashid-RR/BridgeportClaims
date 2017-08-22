using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class ClaimsWithPrescriptionDetailsDto
    {
        [Required]
        public virtual int PrescriptionId { get; set; }
        [Required]
        public virtual int ClaimId { get; set; }
        [Required]
        public virtual string ClaimNumber { get; set; }
        public virtual string PatientName { get; set; }
        [Required]
        public virtual DateTime RxDate { get; set; }
        [Required]
        public virtual string RxNumber { get; set; }
        public virtual string LabelName { get; set; }
        [Required]
        public virtual decimal InvoicedAmount { get; set; }
        public virtual decimal? Outstanding { get; set; }
        public virtual string Payor { get; set; }
    }
}
