using BridgeportClaims.Common.ExpressionManagers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BridgeportClaims.Business.Security
{
    public abstract class SymmetricEncryptor : IEncryptor
    {
        #region Private Members, Ctor

        private readonly SymmetricAlgorithm _cryptoProvider;
        private byte[] _encryptionKeyBytes;

        protected SymmetricEncryptor(
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
                DisposableManager.Using(() => new MemoryStream(), memoryStream =>
                    {
                        return DisposableManager.Using(() => new CryptoStream(
                                memoryStream, _cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write),
                                cryptoStream =>
                                {
                                    return DisposableManager.Using(() => new StreamWriter(cryptoStream), writer =>
                                    {
                                        writer.Write(plainText);
                                        writer.Flush();
                                        cryptoStream.FlushFinalBlock();
                                        return Convert.ToBase64String(
                                            memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                                    });
                                });
                    });
        }

        private byte[] GetEncryptionKeyBytes()
        {
            return _encryptionKeyBytes ?? (_encryptionKeyBytes = Encoding.ASCII.GetBytes(EncryptionKey));
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