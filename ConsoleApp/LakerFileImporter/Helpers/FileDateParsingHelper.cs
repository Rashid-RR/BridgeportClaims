using System;
using System.Globalization;

namespace LakerFileImporter.Helpers
{
    internal class FileDateParsingHelper
    {
        private const char Slash = '/';
        internal string FileName { get; set; }
        internal DateTime LakerFileDate => GetDateTimeParsedFromFileName(FileName);
        internal string FullFileName { get; set; }
        internal static DateTime GetDateTimeParsedFromFileName(string fileName)
        {
            fileName = fileName.StartsWith(Slash.ToString()) ? fileName.TrimStart(Slash) : fileName;
            var date = fileName.Replace("Billing_Claim_File_", string.Empty).Replace(".csv", string.Empty);
            return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
