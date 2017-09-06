using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using LumenWorks.Framework.IO.Csv;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public class CsvReaderProvider : ICsvReaderProvider
    {
        public DataTable ReadCsvFile(string fullFilePath) => DisposableService.Using(() 
            => new CachedCsvReader(
            new StreamReader(fullFilePath), false, '\t'), csv =>
            {
                var table = new DataTable();
                table.Load(csv);
                return table;
            });
    }
}