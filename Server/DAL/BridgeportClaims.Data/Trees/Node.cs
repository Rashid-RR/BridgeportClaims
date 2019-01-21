using System.Collections.Generic;

namespace BridgeportClaims.Data.Trees
{
    public class Node
    {
        internal Node() { }
        public int TreeId { get; internal set; }
        public short TreeLevel { get; internal set; }
        public string NodeName { get; internal set; }
        public string NodeDescription { get; internal set; }
        public int ParentTreeId { get; internal set; }
        public List<Node> Children { get; internal set; }
    }

    public class Tree
    {
        public int TreeId { get; set; }
        public short TreeLevel { get; set; }
        public string NodeName { get; set; }
        public string NodeDescription { get; set; }
        public List<Node> Children { get; set; }
    }
}