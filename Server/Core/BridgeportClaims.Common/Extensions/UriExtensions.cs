using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BridgeportClaims.Common.Disposable;

namespace BridgeportClaims.Common.Extensions
{
    public static class UriExtensions
    {
        public static Uri ToAbsoluteUri(this string _this) => 
            null == _this ? null : new Uri(_this, UriKind.Absolute);

        public static Dictionary<string, string> Parameters(
            this Uri _this) => _this.Query.IsNullOrWhiteSpace()
            ? new Dictionary<string, string>()
            : _this.Query.Substring(1).Split('&').ToDictionary(
                p => p.Split('=')[0],
                p => p.Split('=')[1]
            );

        public static Uri ToTiny(this Uri longUri)
        {
            var request = WebRequest.Create(
                $"http://tinyurl.com/api-create.php?url={UrlEncode(longUri.ToString())}");
            var response = request.GetResponse();
            Uri returnUri = null;
            DisposableService.Using(
                () => new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()),
                reader =>
                {
                    returnUri = new Uri(reader.ReadToEnd());
                });
            return returnUri;
        }

        #region Reflected from System.Web.HttpUtility

        private static string UrlEncode(string str) => str == null ? null : UrlEncode(str, Encoding.UTF8);

        private static string UrlEncode(string str, Encoding e) =>
            str == null ? null : Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));

        private static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            var bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count,
            bool alwaysCreateReturnValue)
        {
            var num = 0;
            var num2 = 0;
            for (var i = 0; i < count; i++)
            {
                var ch = (char) bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsSafe(ch))
                {
                    num2++;
                }
            }

            if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
            {
                return bytes;
            }

            var buffer = new byte[count + (num2 * 2)];
            var num4 = 0;
            for (var j = 0; j < count; j++)
            {
                var num6 = bytes[offset + j];
                var ch2 = (char) num6;
                if (IsSafe(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte) IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte) IntToHex(num6 & 15);
                }
            }

            return buffer;
        }

        internal static bool IsSafe(char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9')
            {
                return true;
            }
            switch (ch)
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }
        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }
        #endregion
    }
}