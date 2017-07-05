﻿using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public DateTime? RxDate { get; set; }
        public string RxNumber { get; set; }
        public string LabelName { get; set; }
        public string BillTo { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? Outstanding { get; set; }
        public DateTime? InvoiceDate { get; set; }
    }
}