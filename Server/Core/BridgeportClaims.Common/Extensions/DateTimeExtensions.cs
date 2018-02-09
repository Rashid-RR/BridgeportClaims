using System;

namespace BridgeportClaims.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private const string Mst = "Mountain Standard Time";
        private const string DateFormat = "MM/dd/yyyy";

        public static DateTime ToMountainTime(this DateTime utc)
        {
            var mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId
                (utc, Mst);
            return mountain;
        }

        public static DateTime? ToNullableFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace()) return null;
            var dt = DateTime.ParseExact(_this, DateFormat, null);
            return dt;
        }

        public static DateTime ToFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(_this));
            var dt = DateTime.ParseExact(_this, DateFormat, null);
            return dt;
        }
    }
}