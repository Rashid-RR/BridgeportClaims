namespace BridgeportClaims.ExcelDataReader.Exceptions.Core.BinaryFormat
{
    /// <summary>
    /// Represents a string stored in SST
    /// </summary>
    internal class XlsBiffLabelSstCell : XlsBiffBlankCell
    {
        internal XlsBiffLabelSstCell(byte[] bytes, uint offset, int biffVersion)
            : base(bytes, offset, biffVersion)
        {
        }

        /// <summary>
        /// Gets the index of string in Shared String Table
        /// </summary>
        public uint SstIndex => ReadUInt32(0x6);
    }
}