namespace BridgeportClaims.Business.PrescriptionReports
{
    public interface IPrescriptionReportFactory
    {
        string GeneratePrescriptionReport(int claimId);
    }
}