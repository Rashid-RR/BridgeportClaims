using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class CollectionsBonusDto
    {
        public IList<CollectionsBonusResultsDto> Results { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalBonusAmount { get; set; }
    }
}