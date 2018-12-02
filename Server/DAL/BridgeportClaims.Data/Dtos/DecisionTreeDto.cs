using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DecisionTreeDto
    {
        [Required]
        public string TreeNode { get; set; }
        [Required]
        public string TreePath { get; set; }
        [Required]
        public short TreeLevel { get; set; }
        [Required]
        public int TreeId { get; set; }
        [Required]
        public string NodeName { get; set; }
        [Required]
        public string NodeDescription { get; set; }
    }
}