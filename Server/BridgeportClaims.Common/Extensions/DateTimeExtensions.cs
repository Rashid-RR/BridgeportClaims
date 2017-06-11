using System;
using System.Globalization;

namespace BridgeportClaims.Common.Extensions
{
    /// <summary>
    /// DateTime Extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        ///  
        /// <summary>
        /// Private Members
        /// </summary>

        #region Elapsed extension
        /// <summary>
        /// Elapses the time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan Elapsed(this DateTime datetime)
        {
            return DateTime.Now - datetime;
        }
        #endregion

        #region Week of year
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek)
        {
            var ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            var dateinf = new DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, CalendarWeekRule weekrule)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime)
        {
            var dateinf = new DateTimeFormatInfo();
            var weekrule = dateinf.CalendarWeekRule;
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        #endregion

        #region Get Datetime for Day of Week
        /// <summary>
        /// Gets the date time for day of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="day">The day.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day, DayOfWeek firstDayOfWeek)
        {
            var current = DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek);
            var resultday = DaysFromFirstDayOfWeek(day, firstDayOfWeek);
            return datetime.AddDays(resultday - current);
        }
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return GetDateTimeForDayOfWeek(datetime, day, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime)
        {
            var dateinf = new DateTimeFormatInfo();
            var firstDayOfWeek = dateinf.FirstDayOfWeek;
            return FirstDateTimeOfWeek(datetime, firstDayOfWeek);
        }
        /// <summary>
        /// Firsts the date time of week.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            return datetime.AddDays(-DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek));
        }

        /// <summary>
        /// Days from first day of week.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        private static int DaysFromFirstDayOfWeek(DayOfWeek current, DayOfWeek firstDayOfWeek)
        {
            //Sunday = 0,Monday = 1,...,Saturday = 6
            var daysbetween = current - firstDayOfWeek;
            if (daysbetween < 0) daysbetween = 7 + daysbetween;
            return daysbetween;
        }
        #endregion

        #region Validation Methods

        /// <summary>
        /// Returns whether the DateTime is on a Weekend.
        /// </summary>
        /// <returns>Returns whether the DateTime is on a Weekend.</returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns whether the DateTime is on a Week Day.
        /// </summary>
        /// <returns>Returns whether the DateTime is on a Week Day.</returns>
        public static bool IsWeekDay(this DateTime dateTime)
        {
            return !dateTime.IsWeekend();
        }

        public static bool IsNullOrEmpty(this DateTime dateTime)
        {
            return (dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
        }

        public static bool IsNullOrEmpty(this DateTime? dateTime)
        {
            return (dateTime == null || dateTime == DateTime.MinValue || dateTime == new DateTime(1900, 1, 1));
        }
        #endregion

        #region Conversion Methods
        public static string ToShortDateString(this DateTime? value)
        {
            return value.ToDateTime().ToShortDateString();
        }

        public static DateTime ToDateTime(this DateTime? dateTime)
        {
            return (dateTime == null) ? DateTime.MinValue : Convert.ToDateTime(dateTime);
        }

        public static string ToMonthString(this DateTime dateTime)
        {
            return dateTime.ToMonthString(new CultureInfo("en-US"));
        }
        public static string ToMonthString(this DateTime dateTime, CultureInfo cultureInfo)
        {
            return dateTime.ToString("MMMM", cultureInfo);
        }
        #endregion

        public static string GetValueOrDefaultToString(this DateTime? datetime, string defaultvalue)
        {
            return datetime == null ? defaultvalue : datetime.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetValueOrDefaultToString(this DateTime? datetime, string format, string defaultvalue)
        {
            return datetime == null ? defaultvalue : datetime.Value.ToString(format);
        }

        /// <summary>
        /// Use to set the time on a DateTime object without modifying the date. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? SetTime(this DateTime? value, string time)
        {
            if (value != null && time != null)
            {
                var newDateTime = value.ToDateTime();
                var dateTime = $"{newDateTime.ToShortDateString()} {time}";
                return DateTime.TryParse(dateTime, out newDateTime) ? newDateTime : value;
            }
            else
                return value;
        }

        public static DateTime? SetTime(this DateTime? value, int hour, int minute)
        {
            if (value == null) return null;
            var time = $"{hour}:{minute}";
            var newDateTime = value.ToDateTime();
            var dateTime = $"{newDateTime.ToShortDateString()} {time}";
            return DateTime.TryParse(dateTime, out newDateTime) ? newDateTime : value;
        }

        public static DateTime AddTime(this DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        public static DateTime AddTime(this DateTime date, DateTime? time)
        {
            return time.HasValue ? date.AddTime(time.Value) : date;
        }

        public static DateTime? AddTime(this DateTime? date, DateTime? time, bool returnNullIfNoDate = true)
        {
            if (date.HasValue)
            {
                return date.Value.AddTime(time);
            }
            if (time.HasValue && !returnNullIfNoDate)
                return time;
            return null;
        }

        /// <summary>
        /// Use to set the date on a DateTime object without modifying the time. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? SetDate(this DateTime? value, DateTime? date)
        {
            if (value == null) return null;
            DateTime newDateTime;
            var dateTime = $"{date.ToDateTime().ToShortDateString()} {value.ToDateTime().ToShortTimeString()}";
            return DateTime.TryParse(dateTime, out newDateTime) ? newDateTime : value;
        }

        public static DateTime JustDateNoTime(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month,
            dateTime.Day);

        public static TimeSpan JustTime(this DateTime dateTime) => new TimeSpan(dateTime.Hour, dateTime.Minute,
            dateTime.Second);


        public static DateTime Noon(this DateTime dateTime)
        {
            var time = ((DateTime?) dateTime).SetTime("12:00 pm");
            if (time != null)
                return (DateTime) time;
            throw new Exception($"Could not set the time to Noon on Date Time: {dateTime:F}");
        }

        /// <summary>
        /// Gets a DateTime representing midnight on the current date
        /// </summary>
        public static DateTime Midnight(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month,
            dateTime.Day);

        /// <summary>
        /// Gets a DateTime representing midnight on the first day of the current date's month
        /// </summary>
        /// <param name="dateTime">The current date</param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime? Midnight(this DateTime? dateTime)
        {
            if (dateTime == null) return null;
            var castDateTime = dateTime.ToDateTime();
            return new DateTime(castDateTime.Year, castDateTime.Month, castDateTime.Day);
        }

        public static string MonthName(this DateTime dateTime) => dateTime.ToString("MMMM");

        public static DateTime NextMonth(this DateTime dateTime) => dateTime.AddMonths(1);

        public static DateTime LastMonth(this DateTime dateTime) => dateTime.AddMonths(-1);

        public static string Meridiem(this DateTime dateTime)
        {
            var time = dateTime.ToString("t").ToLower();
            return time.Substring(time.IndexOf(' '));
        }

        public static int GetHour(this DateTime dateTime)
        {
            if (dateTime.Hour == 0)
                return 12;
            return (dateTime.Hour > 12) ? (dateTime.Hour - 12) : dateTime.Hour;
        }

        public static DateTime? AddHours(this DateTime? dateTime, double value)
        {
            if (dateTime == null) return null;
            var castDateTime = dateTime.ToDateTime();
            return castDateTime.AddHours(value);
        }

        public static DateTime StartOfDay(this DateTime value) => value.Midnight();
        public static DateTime? StartOfDay(this DateTime? value) => value?.Midnight();

        public static DateTime EndOfDay(this DateTime value)
        {
            return value.Midnight().AddHours(24).AddSeconds(-1);
        }
        public static DateTime? EndOfDay(this DateTime? value)
        {
            if (value == null)
                return null;
            return value.ToDateTime().Midnight().AddHours(24).AddSeconds(-1);
        }

        public static DateTime LocalToUtc(this DateTime localDateTime)
        {
            if (localDateTime == DateTime.MinValue)
                return DateTime.MinValue;

            DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);

            return localDateTime.ToUniversalTime();
        }

        public static DateTime UtcToLocal(this DateTime universalTime)
        {
            if (universalTime == DateTime.MinValue)
                return DateTime.MinValue;

            DateTime.SpecifyKind(universalTime, DateTimeKind.Utc);

            return universalTime.ToLocalTime();
        }
    }
}