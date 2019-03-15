using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DecisionTreeChoiceModalDto
    {
        public DecisionTreeChoiceModalHeaderDto DecisionTreeChoiceModalHeader { get; set; }
        public IEnumerable<DecisionTreeChoiceModalPathDto> DecisionTreeChoiceModalPaths { get; set; }
    }
}