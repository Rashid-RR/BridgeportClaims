using System;
using ProtoBuf;

namespace BridgeportClaims.Data.Dtos
{
    [ProtoContract]
    public class ClaimsUserHistoryDto
    {
        [ProtoMember(1)]
        public int ClaimId { get; set; }
        [ProtoMember(2)]
        public string ClaimNumber { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string InjuryDate { get; set; }
        [ProtoMember(5)]
        public string Carrier { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedOnUtc { get; set; }
    }
}