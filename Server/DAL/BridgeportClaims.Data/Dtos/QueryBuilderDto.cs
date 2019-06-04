using System;

namespace BridgeportClaims.Data.Dtos
{
    public class QueryBuilderDto
    {
        private DateTime? _dateSubmitted;
        public int ClaimId { get; set; }
        public int PrescriptionId { get; set; }
        public int? PrescriptionPaymentId { get; set; }
        public string GroupName { get; set; }
        public string Pharmacy { get; set; }
        public string StateCode { get; set; }
        public string DateSubmitted
        {
            get => $"{_dateSubmitted:MM/dd/yyyy}";
            set => _dateSubmitted = Convert.ToDateTime(value);
        }
        public decimal Billed { get; set; }
        public decimal Payable { get; set; }
        public decimal? Collected { get; set; }
        public string Prescriber { get; set; }
        public string PatientLast { get; set; }
        public string PatientFirst { get; set; }
        public string ClaimNumber { get; set; }
        public bool IsAttorneyManaged { get; set; }
        public string AttorneyName { get; set; }
    }
}