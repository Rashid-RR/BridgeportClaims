namespace BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat
{
    /// <summary>
    /// Represents MSO Drawing record
    /// </summary>
    internal class XlsBiffMsoDrawing : XlsBiffRecord
    {
        internal XlsBiffMsoDrawing(byte[] bytes, uint offset)
            : base(bytes, offset)
        {
        }
    }
}
