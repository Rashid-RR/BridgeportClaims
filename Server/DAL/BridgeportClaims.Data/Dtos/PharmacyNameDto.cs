using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PharmacyNameDto
    {
        public string Nabp { get; set; }
        public string PharmacyName { get; set; }
    }
}