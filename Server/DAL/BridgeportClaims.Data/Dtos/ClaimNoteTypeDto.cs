using ProtoBuf;
using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("ClaimNoteType")]
    [ProtoContract]
    public sealed class ClaimNoteTypeDto
    {
        [SQLinqColumn("ClaimNoteTypeID")]
        [ProtoMember(1)]
        public int ClaimNoteTypeId { get; set; }
        [SQLinqColumn("TypeName")]
        [ProtoMember(2)]
        public string TypeName { get; set; }
    }
}