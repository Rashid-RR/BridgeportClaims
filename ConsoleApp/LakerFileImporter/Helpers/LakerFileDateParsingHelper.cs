using System;
using System.Globalization;

namespace LakerFileImporter.Helpers
{
    internal class LakerFileDateParsingHelper
    {
        private const char Slash = '/';
        internal string FileName { get; set; }
        internal DateTime FileDate => GetDateTimeParsedFromFileName(FileName);
        internal string FullFileName { get; set; }
        internal static DateTime GetDateTimeParsedFromFileName(string fileName)
        {
            fileName = fileName.StartsWith(Slash.ToString()) ? fileName.TrimStart(Slash) : fileName;
            if (fileName.StartsWith("Billing_Claim_File_"))
            {
                var date = fileName.Replace("Billing_Claim_File_", string.Empty).Replace(".csv", string.Empty);
                return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else
            {
                if (!fileName.StartsWith("ENVexport_BPC_"))
                {
                    throw new Exception("File name is not in the right format.");
                }
                var date = fileName.Substring(14, 8);
                return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
        }
    }
}
