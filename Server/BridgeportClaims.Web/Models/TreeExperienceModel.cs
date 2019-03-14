namespace BridgeportClaims.Web.Models
{
    public sealed class TreeExperienceModel
    {
        public int LeafTreeId { get; set; }
        public int? ClaimId { get; set; }
        public byte EpisodeTypeId { get; set; }
        public string PharmacyNabp { get; set; }
        public string RxNumber { get; set; }
        public string EpisodeText { get; set; }
    }
}