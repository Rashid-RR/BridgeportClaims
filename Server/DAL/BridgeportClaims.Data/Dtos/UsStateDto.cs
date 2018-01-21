using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class UsStateDto
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}