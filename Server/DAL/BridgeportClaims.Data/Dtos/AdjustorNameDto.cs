using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Adjustor")]
    public sealed class AdjustorNameDto
    {
        [SQLinqColumn("AdjustorID")]
        public int AdjustorId { get; set; }
        [SQLinqColumn("AdjustorName")]
        public string AdjustorName { get; set; }
    }
}