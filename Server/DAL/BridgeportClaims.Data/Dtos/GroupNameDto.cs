using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Payor")]
    public sealed class GroupNameDto
    {
        [SQLinqColumn("GroupName")]
        public string GroupName { get; set; }
    }
}