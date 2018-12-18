using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DuplicateClaimResultsDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int ClaimId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ClaimNumber { get; set; }
        public string PersonCode { get; set; }
        public string GroupName { get; set; }
    }
}
