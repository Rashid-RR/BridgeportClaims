namespace BridgeportClaims.Web.Models
{
    public sealed class DecisionTreeModel
    {
        public int ParentTreeId { get; set; } = 1;
        public string NodeName { get; set; }
        public string NodeDescription { get; set; } = null;
    }
}