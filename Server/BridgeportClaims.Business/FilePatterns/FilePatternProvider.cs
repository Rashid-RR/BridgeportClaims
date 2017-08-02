using System;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Business.FilePatterns
{
    public static class FilePatternProvider
    {
        public static string GetLakerFilePattern(int year, int month, int day)
        {
            if (month < 1 || month > 12)
                throw new Exception($"Error. The number {month} is not a valid month");
            if (day < 1 || day > 31)
                throw new Exception($"Error. The number {day} is not a valid day");
            if (IntLength(year) != 4)
                throw new Exception("Error. The year parameter must have a length of four");
            var lakerFileNamePattern = cs.GetAppSetting(c.LakerFilePatternKeyName);
            lakerFileNamePattern =
                lakerFileNamePattern.Replace("yyyyMMdd", $"{year}{GetMonthString(month)}{GetDayString(day)}");
            return lakerFileNamePattern;
        }

        public static string GetPaymentFilePattern(int year, int month)
        {
            var paymentFileNamePattern = cs.GetAppSetting(c.PaymentFilePatternKeyName);
            return paymentFileNamePattern;
        }

        private static string GetDayString(int day) => day < 10 ? $"0{day}" : day.ToString();

        private static string GetMonthString(int month) => month < 10 ? $"0{month}" : month.ToString();

        private static int IntLength(int i)
        {
            if (i < 0)
                throw new ArgumentOutOfRangeException();
            if (i == 0)
                return 1;
            return (int)Math.Floor(Math.Log10(i)) + 1;
        }
    }
}