using System;
using System.Text;
using System.Web;

namespace BridgeportClaims.Common.Extensions
{
    public static class UrlEncodingExtensions
    {
        public static string Base64ForUrlEncode(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return null;
            }
            var bytes = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(bytes);
        }

        public static string Base64ForUrlDecode(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return null;
            }
            var bytes = HttpServerUtility.UrlTokenDecode(str);
            if (null == bytes)
            {
                throw new Exception("Could not decode token.");
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}