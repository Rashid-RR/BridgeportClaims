namespace BridgeportClaims.Services.Constants
{
    public class ConstantsService : IConstantsService
    {
        private const string BridgeportClaimsDatabaseConnectionString = "BridgeportClaimsConnectionString";
        public string DbConnStr => BridgeportClaimsDatabaseConnectionString;
    }
}