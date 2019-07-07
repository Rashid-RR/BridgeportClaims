using System.Data;
using BridgeportClaims.Common.Enums;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public interface ICsvReaderProvider
    {
        DataTable ReadLakerCsvFile(string fullFilePath, bool useCsvTools, FileSource fileSource);
    }
}