using System;
using System.Globalization;

namespace BridgeportClaims.Common.Extensions
{
    /// <summary>
    /// Decimal Extensions
    /// </summary>
    public static class DecimalExtensions
    {

        #region Conversion Methods
        public static string ToMoneyString(this decimal value)
        {
            return value.ToMoneyString(new CultureInfo("en-US", false));
        }
        public static string ToMoneyString(this decimal value, CultureInfo cultureInfo)
        {
            return value.ToString("c", cultureInfo);
        }
        #endregion

        public static string Truncate(this decimal value)
        {
            return value.Truncate(false);
        }

        public static string Truncate(this decimal value, bool insertCommas)
        {
            var decimalStartPos = 0;

            var returnString = insertCommas
                ? $"{value:n}"
                : value.ToString(CultureInfo.InvariantCulture);

            decimalStartPos = returnString.IndexOf(".", StringComparison.Ordinal);

            if (decimalStartPos > 0)
                returnString = returnString.Substring(0, decimalStartPos);

            return returnString;
        }

        #region PercentageOf calculations

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this decimal number, int percent)
        {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this decimal position, int total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this decimal number, decimal percent)
        {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this decimal position, decimal total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        /// <summary>
        /// The numbers percentage
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="percent">The percent.</param>
        /// <returns>The result</returns>
        public static decimal PercentageOf(this decimal number, long percent)
        {
            return (decimal)(number * percent / 100);
        }

        /// <summary>
        /// Percentage of the number.
        /// </summary>
        /// <param name="percent">The percent</param>
        /// <param name="number">The Number</param>
        /// <returns>The result</returns>
        public static decimal PercentOf(this decimal position, long total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)position / (decimal)total * 100;
            return result;
        }

        #endregion
    }
}