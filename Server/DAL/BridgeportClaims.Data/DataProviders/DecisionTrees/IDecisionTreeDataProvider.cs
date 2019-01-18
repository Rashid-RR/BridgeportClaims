using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public interface IDecisionTreeDataProvider
    {
        DecisionTreeDto InsertDecisionTree(int parentTreeId, string nodeName, string nodeDescription,
            string modifiedByUserId);
        IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId);
        DecisionTreeListDto GetDecisionTreeList(string searchText, string sort, string sortDirection, int page,
            int pageSize);
    };
  
}