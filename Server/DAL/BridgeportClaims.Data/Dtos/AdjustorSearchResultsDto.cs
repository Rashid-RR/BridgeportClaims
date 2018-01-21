using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class AdjustorSearchResultsDto
    {
        public int AdjustorId { get; set; }
        public string AdjustorName { get; set; }
    }
}