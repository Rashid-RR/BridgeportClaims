using System;
using ProtoBuf;

namespace BridgeportClaims.Data.Dtos
{
    [ProtoContract]
    public sealed class QueryBuilderDto
    {
        private DateTime _dt;
        private DateTime _dateSubmitted;
        private DateTime _dateFilled;
        private DateTime? _invoiceDate;
        [ProtoMember(1)]
        public int ClaimId { get; set; }
        [ProtoMember(2)]
        public int PrescriptionId { get; set; }
        [ProtoMember(3)]
        public int? PrescriptionPaymentId { get; set; }
        [ProtoMember(4)]
        public string GroupName { get; set; }
        [ProtoMember(5)]
        public string Pharmacy { get; set; }
        [ProtoMember(6)]
        public string StateCode { get; set; }
        [ProtoMember(7)]
        public string DateSubmitted
        {
            get => $"{_dateSubmitted:MM/dd/yyyy}";
            set => _dateSubmitted = Convert.ToDateTime(value);
        }
        [ProtoMember(8)]
        public decimal Billed { get; set; }
        [ProtoMember(9)]
        public decimal Payable { get; set; }
        [ProtoMember(10)]
        public decimal? Collected { get; set; }
        [ProtoMember(11)]
        public string Prescriber { get; set; }
        [ProtoMember(12)]
        public string PatientLast { get; set; }
        [ProtoMember(13)]
        public string PatientFirst { get; set; }
        [ProtoMember(14)]
        public string ClaimNumber { get; set; }
        [ProtoMember(15)]
        public bool IsAttorneyManaged { get; set; }
        [ProtoMember(16)]
        public string AttorneyName { get; set; }
        [ProtoMember(17)]
        public string LabelName { get; set; }
        [ProtoMember(18)]
        public string RxNumber { get; set; }
        [ProtoMember(19)]
        public string DateFilled
        {
            get => $"{_dateFilled:MM/dd/yyyy}";
            set => _dateFilled = Convert.ToDateTime(value);
        }
        [ProtoMember(20)]
        public string Ndc { get; set; }
        [ProtoMember(21)]
        public string InvoiceNumber { get; set; }
        [ProtoMember(22)]
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
