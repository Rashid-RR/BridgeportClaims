using BridgeportClaims.Data.Enums;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public interface IPrescriptionsProvider
    {
        EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId);
    }
}