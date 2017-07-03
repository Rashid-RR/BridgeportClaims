using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimNoteTypesDto
    {
        public int ClaimNoteTypeId { get; set; }
        public string TypeName { get; set; }
    }
}