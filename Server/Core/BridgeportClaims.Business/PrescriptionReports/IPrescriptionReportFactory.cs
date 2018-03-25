using System.Data;

namespace BridgeportClaims.Business.PrescriptionReports
{
    public interface IPrescriptionReportFactory
    {
        string GetLastNameAndFirstNameFromClaimId(int claimId);
        DataTable GenerateBillingStatementDataTable(int claimId);
    }
}