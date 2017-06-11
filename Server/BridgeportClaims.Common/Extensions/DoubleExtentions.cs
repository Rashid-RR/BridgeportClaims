using System.Globalization;

namespace BridgeportClaims.Common.Extensions
{
    /// <summary>
    /// Double Extentions - JHE
    /// </summary>
    public static class DoubleExtentions
    {
        #region Conversion Methods
        public static string ToMoneyString(this double value)
        {
            return value.ToMoneyString(new CultureInfo("en-US", false));
        }
        public static string ToMoneyString(this double value, CultureInfo cultureInfo)
        {
            return value.ToString("c", cultureInfo);
        }
        #endregion
    }
}
