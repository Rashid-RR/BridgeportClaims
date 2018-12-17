using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("PrescriptionNoteType")]
    public sealed class PrescriptionNoteTypesDto
    {
        [SQLinqColumn("PrescriptionNoteTypeID")]
        public int PrescriptionNoteTypeId { get; set; }
        [SQLinqColumn("TypeName")]
        public string TypeName { get; set; }
    }
}
