using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Payor")]
    public sealed class PayorsDto
    {
        [SQLinqColumn("PayorID")]
        public int PayorId { get; set; }
        [SQLinqColumn("GroupName")]
        public string GroupName { get; set; }
    }
}