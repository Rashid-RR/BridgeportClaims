using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DashboardDto
    {
        public DateTime? LastWorkDate { get; set; }
        public int? TotalImagesScanned { get; set; }
        public int? TotalImagesIndexed { get; set; }
        public int? TotalImagesRemaining { get; set; }
        public int? DiariesAdded { get; set; }
        public int? TotalDiariesResolved { get; set; }
        public int? TotalDiariesUnResolved { get; set; }
        public int? NewClaims { get; set; }
        public int? NewPrescriptions { get; set; }
        public int? NewReversedPrescriptions { get; set; }
        public int? NewInvoicesPrinted { get; set; }
        public decimal? NewPaymentsPosted { get; set; }
        public int? NewEpisodes { get; set; }
        public int? TotalResolvedEpisodes { get; set; }
        public int? TotalUnresolvedEpisodes { get; set; }
        public bool FileWatcherHealthy { get; set; }
    }
}