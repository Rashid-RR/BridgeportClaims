using System;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.Trees
{
    public static class HierarchyService
    {
        public static Tree ToHierarchy(this IEnumerable<DecisionTreeDto> source, int rootTreeId)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (default == rootTreeId)
            {
                throw new ArgumentNullException(nameof(rootTreeId));
            }
            var treeData = source as DecisionTreeDto[] ?? source.ToArray();
            var root = treeData.SingleOrDefault(x => x.TreeId == rootTreeId);
            if (null == root)
            {
                throw new ArgumentNullException(nameof(root));
            }
            var tree = new Tree
            {
                TreeId = root.TreeId,
                NodeName = root.NodeName,
                NodeDescription = root.NodeDescription,
                TreeLevel = root.TreeLevel,
                Children = Descend(treeData, root.TreeId)
            };
            return tree;
        }

        private static List<Node> Descend(DecisionTreeDto[] treeData, int parentTreeId)
        {
            return treeData.Where(x => x.ParentTreeId == parentTreeId)
                .Select(node => new Node
                {
                    TreeId = node.TreeId,
                    NodeName = node.NodeName,
                    NodeDescription = node.NodeDescription,
                    ParentTreeId = node.ParentTreeId,
                    TreeLevel = node.TreeLevel,
                    Children = Descend(treeData, node.TreeId)
                }).ToList();
        }
    }
}