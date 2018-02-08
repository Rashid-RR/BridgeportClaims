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
    }
}