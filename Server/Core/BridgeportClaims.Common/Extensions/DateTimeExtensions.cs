using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BridgeportClaims.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private const string Mst = "Mountain Standard Time";
        private const string SingleDigitDateFormat = "M/d/yyyy";
        private const string DoubleDigitDateFormat = "MM/dd/yyyy";
        private const string RegExPattern = @"^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$";

        public static DateTime ToMountainTime(this DateTime utc)
        {
            var mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId
                (utc, Mst);
            return mountain;
        }

        public static DateTime? ToNullableFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace()) return null;
            var match = Regex.IsMatch(_this, RegExPattern);
            DateTime? dt = match ? DateTime.Parse(_this) : _this.TryParseExact();
            return dt;
        }

        public static DateTime TryParseExact(this string _this)
        {
            if (DateTime.TryParseExact(_this, SingleDigitDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var sdt))
            {
                return sdt;
            }
            if (DateTime.TryParseExact(_this, DoubleDigitDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var ddt))
            {
                return ddt;
            }
            throw new Exception("Could not parse Date Time format.");
        }

        public static DateTime ToFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(_this));
            var match = Regex.IsMatch(_this, RegExPattern);
            var dt = match ? DateTime.Parse(_this) : _this.TryParseExact();
            return dt;
        }
    }
}