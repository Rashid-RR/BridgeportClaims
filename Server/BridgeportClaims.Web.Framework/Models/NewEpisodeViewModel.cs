namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class NewEpisodeViewModel
    {
        public int? ClaimId { get; set; }
        public byte? EpisodeTypeId { get; set; }
        public string PharmacyNabp { get; set; }
        public string RxNumber { get; set; }
        public string EpisodeText { get; set; }
    }
}