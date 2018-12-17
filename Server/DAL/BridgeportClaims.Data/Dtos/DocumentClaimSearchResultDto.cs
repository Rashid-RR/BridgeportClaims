namespace BridgeportClaims.Data.Dtos
{
    public sealed class DocumentClaimSearchResultDto
    {
        public int ClaimId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ClaimNumber { get; set; }
        public string GroupNumber { get; set; }
    }
}
