using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PrescriptionNoteTypesDto
    {
        public int PrescriptionNoteTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
