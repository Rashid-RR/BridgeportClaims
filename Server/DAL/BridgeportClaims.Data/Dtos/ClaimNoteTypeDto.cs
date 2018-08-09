using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("ClaimNoteType")]
    public sealed class ClaimNoteTypeDto
    {
        [SQLinqColumn("ClaimNoteTypeID")]
        public int ClaimNoteTypeId { get; set; }
        [SQLinqColumn("TypeName")]
        public string TypeName { get; set; }
    }
}