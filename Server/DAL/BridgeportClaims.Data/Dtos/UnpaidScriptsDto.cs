using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class UnpaidScriptsDto
    {
        [Required]
        public int PrescriptionId { get; set; }
        [Required]
        public int ClaimId { get; set; }
        [StringLength(312)]
        public string PatientName { get; set; }
        [Required]
        [StringLength(255)]
        public string ClaimNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public decimal InvAmt { get; set; }
        [Required]
        [StringLength(100)]
        public string RxNumber { get; set; }
        [Required]
        public DateTime RxDate { get; set; }
        [StringLength(25)]
        public string LabelName { get; set; }
        [Required]
        [StringLength(255)]
        public string InsuranceCarrier { get; set; }
        [Required]
        [StringLength(2)]
        public string PharmacyState { get; set; }
        [StringLength(255)]
        public string AdjustorName { get; set; }
        [StringLength(30)]
        public string AdjustorPhone { get; set; }
    }
}