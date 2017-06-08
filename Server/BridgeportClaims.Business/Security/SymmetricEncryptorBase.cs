using BridgeportClaims.Common.ExpressionManagers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BridgeportClaims.Business.Security
{
    public abstract class SymmetricEncryptorBase : IEncryptor
    {
        #region Private Members, Ctor

        private readonly SymmetricAlgorithm _cryptoProvider;
        private byte[] _myBytes;

        protected SymmetricEncryptorBase(
            SymmetricAlgorithm cryptoProvider)
        {
            _cryptoProvider = cryptoProvider;
        }
        
        #endregion

        public string EncryptionKey { get; set; }

        /// <summary>
        /// Encrypt Plain Text
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            var bytes = GetEncryptionKeyBytes();
            return
                DisposableManager.Using(() => new MemoryStream(),
                    memoryStream =>
                    {
                        var encryptor = _cryptoProvider
                            .CreateEncryptor(bytes, bytes);

                        using (var cryptoStream = new CryptoStream(
                            memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var writer = new StreamWriter(cryptoStream))
                            {
                                writer.Write(plainText);
                                writer.Flush();
                                cryptoStream.FlushFinalBlock();
                                return Convert.ToBase64String(
                                    memoryStream.GetBuffer(),
                                    0,
                                    (int) memoryStream.Length);
                            }
                        }
                    });
        }

        private byte[] GetEncryptionKeyBytes()
        {
            if (_myBytes == null)
                _myBytes = Encoding.ASCII.GetBytes(EncryptionKey);

            return _myBytes;
        }

        public string Decrypt(string encryptedText)
        {
            var bytes = GetEncryptionKeyBytes();
            using (var memoryStream = new MemoryStream(
                Convert.FromBase64String(encryptedText)))
            {
                var decryptor = _cryptoProvider
                    .CreateDecryptor(bytes, bytes);
                using (var cryptoStream = new CryptoStream(
                    memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

    }
}