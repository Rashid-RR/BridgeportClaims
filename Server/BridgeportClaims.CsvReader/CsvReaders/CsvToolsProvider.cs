using DataAccess;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public class CsvToolsProvider : ICsvToolsProvider
    {
        public MutableDataTable ReadCsvFile(string fullFilePath)
        {
            var dt = DataTable.New.ReadCsv(fullFilePath);
            return dt;
        }
    }
}
