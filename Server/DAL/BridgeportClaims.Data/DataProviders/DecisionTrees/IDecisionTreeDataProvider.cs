using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public interface IDecisionTreeDataProvider
    {
        Tuple<IEnumerable<DecisionTreeDto>, int> InsertDecisionTree(int parentTreeId, string nodeName, string modifiedByUserId);
        IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId);
        DecisionTreeListDto GetDecisionTreeList(string searchText, string sort, string sortDirection, int page,
            int pageSize);
        Tuple<IEnumerable<DecisionTreeDto>, int> DeleteDecisionTree(int treeId);
    };
  
}