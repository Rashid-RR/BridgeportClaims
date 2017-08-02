using System.IO;
using BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat;

namespace BridgeportClaims.ExcelDataReader
{
    /// <summary>
    /// ExcelDataReader Class
    /// </summary>
    internal class ExcelBinaryReader : ExcelDataReader<XlsWorkbook, XlsWorksheet>
    {
        public ExcelBinaryReader(Stream stream, ExcelReaderConfiguration configuration)
            : base(configuration)
        {
            Workbook = new XlsWorkbook(stream, Configuration.FallbackEncoding);

            // By default, the data reader is positioned on the first result.
            Reset();
        }

        public override void Close()
        {
            base.Close();
            Workbook = null;
        }
    }
}
