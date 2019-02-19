using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Payor")]
    public sealed class PayorSearchResultsDto
    {
        [SQLinqColumn("PayorId")]
        public int PayorId { get; set; }
        [SQLinqColumn("GroupName")]
        public string GroupName { get; set; }
    }
}