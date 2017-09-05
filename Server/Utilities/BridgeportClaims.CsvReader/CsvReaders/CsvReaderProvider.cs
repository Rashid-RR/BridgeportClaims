using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using LumenWorks.Framework.IO.Csv;

namespace BridgeportClaims.CsvReader.CsvReaders
{
    public class CsvReaderProvider
    {
        public void ReadCsvFile(string fullFilePath)
        {
            DisposableService.Using(() => new CachedCsvReader(
                new StreamReader(fullFilePath), true), csv =>
            {
                var table = new DataTable();
                table.Load(csv);
            });
        }
    }
}