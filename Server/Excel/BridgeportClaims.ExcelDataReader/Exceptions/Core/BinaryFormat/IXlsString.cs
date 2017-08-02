using System.Text;

namespace BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat
{
    internal interface IXlsString
    {
        /// <summary>
        /// Gets the string value. Encoding is only used with BIFF2-5 byte strings.
        /// </summary>
        string GetValue(Encoding encoding);
    }
}