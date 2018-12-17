namespace BridgeportClaims.Data.DataProviders.Utilities
{
    public interface IUtilitiesProvider
    {
        int ReseedTableAndGetSeedValue(string tableName);
    }
}