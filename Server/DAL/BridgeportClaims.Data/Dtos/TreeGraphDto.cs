namespace BridgeportClaims.Data.Dtos
{
    public class TreeGraphDto
    {
        public string TreePath { get; set; }
        public short TreeLevel { get; set; }
        public int TreeId { get; set; }
        public string NodeName { get; set; }
        public int ParentTreeId { get; set; }
    }
}