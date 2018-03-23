using BridgeportClaims.Business.Extensions;
using System;
using System.Globalization;
using c = BridgeportClaims.Business.StringConstants.Constants;

namespace BridgeportClaims.Business.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? ToParsedDateTime(this string fileName)
        {
            if (fileName.IsNullOrWhiteSpace()) return null;
            var docDate = fileName?.Substring(3, 8);
            if (docDate.IsNullOrWhiteSpace()) return null;
            var returnDate = DateTime.TryParseExact(docDate, c.FileNameDateParsed,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt :
                (DateTime?) null;
            return returnDate;
        }
    }
}