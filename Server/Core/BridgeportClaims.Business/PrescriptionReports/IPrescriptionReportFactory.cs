using System.Data;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Business.PrescriptionReports
{
    public interface IPrescriptionReportFactory
    {
        BillingStatementDto GetBillingStatementDto(int claimId);
        DataTable GenerateBillingStatementDataTable(int claimId);
    }
}