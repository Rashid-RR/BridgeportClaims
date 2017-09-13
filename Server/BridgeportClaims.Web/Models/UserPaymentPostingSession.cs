using System;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Web.Models
{
    public class UserPaymentPostingSession
    {
        public string SessionId { get; set; }
        public string CacheKey => SessionId.IsNotNullOrWhiteSpace() ? SessionId : GetGuidString();
        public UserPaymentPostingSession()
        {
            Id = Guid.NewGuid();
            PaymentPostings = new List<PaymentPosting>();
        }
        public string UserName { get; set; }
        public string CheckNumber { get; set; }
        public decimal CheckAmount { get; set; }
        public decimal AmountSelected { get; set; }
        public decimal AmountsToPost => PaymentPostings?.Sum(x => x.AmountPosted) ?? 0.00m;
        public decimal AmountRemaining => CheckAmount - AmountsToPost;
        public decimal? LastAmountRemaining { get; set; }
        public bool HasSuspense => null != SuspenseAmountRemaining;
        public decimal? SuspenseAmountRemaining { get; set; }
        public string ToSuspenseNoteText { get; set; }
        public List<PaymentPosting> PaymentPostings { get; set; }

        #region Private Members

        private Guid Id { get; }
        private string GetGuidString() => Id.ToString().IsNotNullOrWhiteSpace() ? Id.ToString() : null;

        #endregion
    }
}