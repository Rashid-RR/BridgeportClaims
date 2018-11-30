using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public interface IDecisionTreeDataProvider
    {
        int InsertDecisionTree(int parentTreeId, string nodeName, string nodeDescription);
        IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId);
    }
}