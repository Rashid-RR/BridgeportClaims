using DataAccess;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public interface ICsvToolsProvider
    {
        MutableDataTable ReadCsvFile(string fullFilePath);
    }
}