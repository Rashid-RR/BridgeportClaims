using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DecisionTreeListResultDto
    {
        public int TreeId { get; set; }
        public string NodeName { get; set; }
        public string NodeDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}