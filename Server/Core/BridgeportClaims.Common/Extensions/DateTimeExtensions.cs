using System;

namespace BridgeportClaims.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToMountainTime(this DateTime utc)
        {
            var mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId
                (utc, "Mountain Standard Time");
            return mountain;
        }

        public static DateTime? ToNullableFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace()) return null;
            var dt = DateTime.ParseExact(_this, "MM/dd/yyyy", null);
            return dt;
        }

        public static DateTime ToFormattedDateTime(this string _this)
        {
            if (_this.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(_this));
            var dt = DateTime.ParseExact(_this, "MM/dd/yyyy", null);
            return dt;
        }
    }
}