using System;
using System.Security.Cryptography;
using System.Text;

namespace BridgeportClaims.Business.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HashAlgorithm _algorithm;

        public PasswordHasher(HashAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public string HashPassword(string emailAddress, string password)
        {
            var plainText = emailAddress + password;
            var plainTextData = Encoding.Default.GetBytes(plainText);
            var hash = _algorithm.ComputeHash(plainTextData);
            return Convert.ToBase64String(hash);
        }
    }
}