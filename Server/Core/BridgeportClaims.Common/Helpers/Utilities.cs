using System;
using System.Linq;

namespace BridgeportClaims.Common.Helpers
{
    public static class Utilities
    {
        private static readonly Random Random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "BCDFGHJKLMNPQRSTVWXYZ0123456789"; // Remove vowels so we don't accidentally produce profanity.
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}