﻿using System.Text;

namespace BridgeportClaims.ExcelDataReader
{
    /// <summary>
    /// Configuration options for an instance of ExcelDataReader.
    /// </summary>
    public class ExcelReaderConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating the encoding to use when the input XLS lacks a CodePage record. Default: cp1252. (XLS BIFF2-5 only)
        /// </summary>
        public Encoding FallbackEncoding { get; set; } = Encoding.GetEncoding(1252);
    }
}
