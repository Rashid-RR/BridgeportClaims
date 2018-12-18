using System;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class BillingStatementDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}