﻿using System.Collections.Generic;

namespace BridgeportClaims.ExcelDataReader.Exceptions.Core
{
    /// <summary>
    /// The common worksheet interface between the binary and OpenXml formats
    /// </summary>
    internal interface IWorksheet
    {
        string Name { get; }

        string CodeName { get; }

        string VisibleState { get; }

        HeaderFooter HeaderFooter { get; }

        int FieldCount { get; }

        IEnumerable<object[]> ReadRows();
    }
}
