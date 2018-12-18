using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("Pharmacy")]
    public sealed class PharmacyNameDto
    {
        [SQLinqColumn("NABP")]
        public string Nabp { get; set; }
        [SQLinqColumn("PharmacyName")]
        public string PharmacyName { get; set; }
    }
}