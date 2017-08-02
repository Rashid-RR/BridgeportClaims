using System.Text;

namespace BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat
{
    /// <summary>
    /// Plain string without backing storage. Used internally
    /// </summary>
    internal class XlsInternalString : IXlsString
    {
        private readonly string _stringValue;

        public XlsInternalString(string value)
        {
            _stringValue = value;
        }

        public string GetValue(Encoding encoding)
        {
            return _stringValue;
        }
    }
}
