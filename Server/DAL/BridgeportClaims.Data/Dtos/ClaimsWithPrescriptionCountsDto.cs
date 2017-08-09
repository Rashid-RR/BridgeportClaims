using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class ClaimsWithPrescriptionCountsDto
    {
        public virtual int ClaimId { get; set; }
        public virtual string ClaimNumber { get; set; }
        public virtual string PatientName { get; set; }
        public virtual string Payor { get; set; }
        public virtual int NumberOfPrescriptions { get; set; }
    }
}
