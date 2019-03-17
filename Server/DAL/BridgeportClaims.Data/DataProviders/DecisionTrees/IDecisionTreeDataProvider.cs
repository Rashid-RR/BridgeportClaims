using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public interface IDecisionTreeDataProvider
    {
        DecisionTreeChoiceModalDto GetDecisionTreeChoiceModal(int episodeId);
        DecisionTreeDto InsertDecisionTree(int parentTreeId, string nodeName, string modifiedByUserId);
        IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId);
        DecisionTreeListDto GetDecisionTreeList(string searchText, string sort, string sortDirection, int page,
            int pageSize);
        int DeleteDecisionTree(int treeId);
        EpisodeBladeDto SaveDecisionTreeChoice(int leafTreeId, int? claimId, byte episodeTypeId,
            string pharmacyNabp, string rxNumber, string episodeText, string modifiedByUserId);
        IEnumerable<TreeGraphDto> GetUpline(int leafTreeId);
    };
  
}