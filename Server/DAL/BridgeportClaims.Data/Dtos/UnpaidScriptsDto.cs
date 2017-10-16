using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class UnpaidScriptsDto
    {
        public int PrescriptionId { get; set; }
        public int ClaimId { get; set; }
        public string Owner { get; set; }
        public DateTime? Created { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvAmt { get; set; }
        public string RxNumber { get; set; }
        public DateTime RxDate { get; set; }
        public string LabelName { get; set; }
        public string InsuranceCarrier { get; set; }
        public string PharmacyState { get; set; }
        public string AdjustorName { get; set; }
        public string AdjustorPhone { get; set; }
    }
}