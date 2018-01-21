using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PayorSearchResultsDto
    {
        public int PayorId { get; set; }
        public string GroupName { get; set; }
    }
}