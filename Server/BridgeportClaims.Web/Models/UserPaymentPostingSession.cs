using System;
using System.Collections.Concurrent;
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
            PrescriptionPosts = new ConcurrentDictionary<int, decimal>();
        }

        public string UserName { get; set; }

        public string SourceConnectionId { get; set; }
        public string DestinationConnectionId { get; set; }

        public ConcurrentDictionary<int, decimal> PrescriptionPosts { get; set; }
    }
}