namespace BridgeportClaims.Web.Models
{
    public sealed class TreePathChoiceModel
    {
        public string SessionId { get; set; }
        public int ParentTreeId { get; set; }
        public int SelectedTreeId { get; set; }
        public string Description { get; set; }
    }
}