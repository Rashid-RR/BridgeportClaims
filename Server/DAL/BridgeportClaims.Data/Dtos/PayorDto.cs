using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Payor")]
    public sealed class PayorDto
    {
        [SQLinqColumn("PayorID")]
        public int PayorId { get; set; }    
        [SQLinqColumn("GroupName")]
        public string Carrier { get; set; }
    }
}