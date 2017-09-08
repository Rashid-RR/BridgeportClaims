using System;
using System.Collections.Generic;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Web.Models
{
    public class UserPaymentPostingSession
    {
        public Guid Id { get; }
        public string CacheKey => Id.ToString().IsNotNullOrWhiteSpace() ? Id.ToString() : null;
        public UserPaymentPostingSession()
        {
            Id = Guid.NewGuid();
            PrescriptionIds = new List<int>();
        }
        public string UserName { get; set; }
        public string SourceConnectionId { get; set; }
        public string DestinationConnectionId { get; set; }
        public string CheckNumber { get; set; }
        public decimal CheckAmount { get; set; }
        public decimal SelectedAmount { get; set; }
        public decimal AmountToPost { get; set; }
        public IList<int> PrescriptionIds { get; set; }
    }
}