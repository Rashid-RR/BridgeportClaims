using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PrescriptionStatusDto
    {
        public int PrescriptionStatusId { get; set; }
        public string StatusName { get; set; }
    }
}