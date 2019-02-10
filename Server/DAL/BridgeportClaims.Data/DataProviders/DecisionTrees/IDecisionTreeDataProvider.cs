using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public interface IDecisionTreeDataProvider
    {
        Guid DecisionTreeHeaderInsert(string userId, int treeRootId, int claimId);
        void DecisionTreeUserPathInsert(Guid sessionId, int parentTreeId, int selectedTreeId, string userId);
        DecisionTreeDto InsertDecisionTree(int parentTreeId, string nodeName, string modifiedByUserId);
        IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId);
        DecisionTreeListDto GetDecisionTreeList(string searchText, string sort, string sortDirection, int page,
            int pageSize);
        int DeleteDecisionTree(int treeId);
    };
  
}