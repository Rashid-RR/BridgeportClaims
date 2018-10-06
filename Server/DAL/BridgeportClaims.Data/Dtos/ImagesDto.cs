using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ImagesDto
    {
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public DateTime RxDate { get; set; }
        public string RxNumber { get; set; }
        public string File { get; set; }
    }
}