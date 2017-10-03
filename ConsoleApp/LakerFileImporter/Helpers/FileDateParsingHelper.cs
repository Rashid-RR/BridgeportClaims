using System;
using System.Globalization;

namespace LakerFileImporter.Helpers
{
    internal class FileDateParsingHelper
    {
        internal string FileName { get; set; }
        internal DateTime FileNameExtractedDate => GetDateTimeParsedFromFileName(FileName);
        internal string FullFileName { get; set; }
        internal static DateTime GetDateTimeParsedFromFileName(string fileName)
        {
            var date = fileName.Replace("Billing_Claim_File_", string.Empty).Replace(".csv", string.Empty);
            return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
