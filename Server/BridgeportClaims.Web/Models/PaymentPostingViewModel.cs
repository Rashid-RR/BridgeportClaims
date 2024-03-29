﻿using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public class PaymentPostingViewModel
    {
        public decimal AmountRemaining { get; set; }
        public List<PaymentPosting> PaymentPostings { get; set; }
        public string SessionId { get; set; }
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}