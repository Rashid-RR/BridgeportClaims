using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class EnvisionFileBytesDto
    {
        [StringLength(255)]
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}