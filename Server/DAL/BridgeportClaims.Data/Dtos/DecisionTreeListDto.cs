using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DecisionTreeListDto
    {
        public int TotalRows { get; set; }
        public IEnumerable<DecisionTreeListResultDto> Results { get; set; }
    }
}