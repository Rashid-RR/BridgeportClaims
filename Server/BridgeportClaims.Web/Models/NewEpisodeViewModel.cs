using System;

namespace BridgeportClaims.Web.Models
{
    [Serializable]
    public sealed class NewEpisodeViewModel
    {
        public int ClaimId { get; set; }
        public int? EpisodeTypeId { get; set; }
        public string PharmacyNabp { get; set; }
        public string RxNumber { get; set; }
        public string EpisodeText { get; set; }
    }
}