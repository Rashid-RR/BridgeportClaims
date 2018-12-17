using System.Data;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public interface ICsvReaderProvider
    {
        DataTable ReadCsvFile(string fullFilePath, bool useCsvTools);
    }
}