using System;

namespace BridgeportClaims.Data.Dtos
{
    public class QueryBuilderDto
    {
        private DateTime _dt;
        private DateTime _dateSubmitted;
        private DateTime _dateFilled;
        private DateTime? _invoiceDate;
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
        public string LabelName { get; set; }
        public string RxNumber { get; set; }
        public string DateFilled
        {
            get => $"{_dateFilled:MM/dd/yyyy}";
            set => _dateFilled = Convert.ToDateTime(value);
        }
        public string Ndc { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate
        {
            get => _invoiceDate.HasValue ? $"{_invoiceDate:MM/dd/yyyy}" : string.Empty;
            set
            {
                if (DateTime.TryParse(value, out _dt))
                {
                    _invoiceDate = _dt;
                }
                else
                {
                    _invoiceDate = null;
                }
            }
        }
    }
}
