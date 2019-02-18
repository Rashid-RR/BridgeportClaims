using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Attorney")]
    public sealed class AttorneyNameDto
    {
        [SQLinqColumn("AttorneyID")]
        public int AttorneyId { get; set; }
        [SQLinqColumn("AttorneyName")]
        public string AttorneyName { get; set; }
    }
}
