using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class GenderDto
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}