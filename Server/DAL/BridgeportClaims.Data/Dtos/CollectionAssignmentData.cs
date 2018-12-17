using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class CollectionAssignmentData
    {
        public IList<PayorDto> LeftCarriers { get; set; }
        public IList<PayorDto> RightCarriers { get; set; }
    }
}