namespace BridgeportClaims.Business.BillingStatement
{
    public interface IBillingStatementProvider
    {
        string GenerateBillingStatementFullFilePath(int claimId, out string fileName);
    }
}