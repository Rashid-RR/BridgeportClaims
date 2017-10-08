using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class DiariesDto
    {
        public int DiaryId { get; set; }
        public int ClaimId { get; set; }
        public int PrescriptionNoteId { get; set; }
        public string Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime FollowUpDate { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public string Type { get; set; }
        public string RxNumber { get; set; }
        public DateTime RxDate { get; set; }
        public string InsuranceCarrier { get; set; }
        public string DiaryNote { get; set; }
    }
}