using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ScriptDto
    {
        public DateTime RxDate { get; set; }
        public string RxNumber { get; set; }
    }
}