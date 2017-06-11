using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BridgeportClaims.Business.Security
{
    public class CryptographyManager : IEncryptor
    {
        private byte[] _keyByte = {};
        //Default Key
        private static string _key = @"#$*JFDJM#)#)*@#";
        //Default initial vector
        private byte[] _ivByte = {0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78};

        public string EncryptionKey { get; set; }

        /// <summary> 
        /// Encrypt text 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value)
        {
            return Encrypt(value, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key)
        {
            return Encrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key, string iv)
        {
            var encryptValue = string.Empty;
            MemoryStream ms = null;
            CryptoStream cs = null;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (var des = new DESCryptoServiceProvider())
                    {
                        byte[] inputByteArray =
                            Encoding.UTF8.GetBytes(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateEncryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        encryptValue = Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    //TODO: write log 
                }
                finally
                {
                    cs?.Dispose();
                    ms?.Dispose();
                }
            }
            return encryptValue;
        }

        /// <summary> 
        /// Decrypt text 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value)
        {
            return Decrypt(value, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key">string key</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value, string key)
        {
            return Decrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Decrypt(string value, string key, string iv)
        {
            string decrptValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                MemoryStream ms = null;
                CryptoStream cs = null;
                value = value.Replace(" ", "+");
                byte[] inputByteArray = new byte[value.Length];
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        inputByteArray = Convert.FromBase64String(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateDecryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        Encoding encoding = Encoding.UTF8;
                        decrptValue = encoding.GetString(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return decrptValue;
        }

        /// <summary> 
        /// Hash enum value 
        /// </summary> 
        public enum HashName
        {
            SHA1 = 1,
            MD5 = 2,
            SHA256 = 4,
            SHA384 = 8,
            SHA512 = 16
        }

        /// <summary> 
        /// Compute Hash 
        /// </summary> 
        /// <param name="plainText">plain text</param> 
        /// <param name="salt">salt string</param> 
        /// <returns>string</returns> 
        public string ComputeHash(string plainText, string salt)
        {
            return ComputeHash(plainText, salt, HashName.SHA512);
        }

        /// <summary> 
        /// Compute Hash 
        /// </summary> 
        /// <param name="plainText">plain text</param> 
        /// <param name="salt">salt string</param> 
        /// <param name="hashName">Hash Name</param> 
        /// <returns>string</returns> 
        public string ComputeHash(string plainText, string salt, HashName hashName)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                // Convert plain text into a byte array. 
                byte[] plainTextBytes = Encoding.ASCII.GetBytes(plainText);
                // Allocate array, which will hold plain text and salt. 
                byte[] plainTextWithSaltBytes = null;
                byte[] saltBytes;
                if (!string.IsNullOrEmpty(salt))
                {
                    // Convert salt text into a byte array. 
                    saltBytes = Encoding.ASCII.GetBytes(salt);
                    plainTextWithSaltBytes =
                        new byte[plainTextBytes.Length + saltBytes.Length];
                }
                else
                {
                    // Define min and max salt sizes. 
                    const int minSaltSize = 4;
                    const int maxSaltSize = 8;
                    // Generate a random number for the size of the salt. 
                    var random = new Random();
                    var saltSize = random.Next(minSaltSize, maxSaltSize);
                    // Allocate a byte array, which will hold the salt. 
                    saltBytes = new byte[saltSize];
                    // Initialize a random number generator. 
                    var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                    // Fill the salt with cryptographically strong byte values. 
                    rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);
                }
                // Copy plain text bytes into resulting array. 
                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }
                // Append salt bytes to the resulting array. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] =
                                        saltBytes[i];
                }
                HashAlgorithm hash = null;
                switch (hashName)
                {
                    case HashName.SHA1:
                        hash = new SHA1Managed();
                        break;
                    case HashName.SHA256:
                        hash = new SHA256Managed();
                        break;
                    case HashName.SHA384:
                        hash = new SHA384Managed();
                        break;
                    case HashName.SHA512:
                        hash = new SHA512Managed();
                        break;
                    case HashName.MD5:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }
                // Compute hash value of our plain text with appended salt. 
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                // Create array which will hold hash and original salt bytes. 
                byte[] hashWithSaltBytes =
                    new byte[hashBytes.Length + saltBytes.Length];
                // Copy hash bytes into resulting array. 
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }
                // Append salt bytes to the result. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
                }
                // Convert result into a base64-encoded string. 
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);
                // Return the result. 
                return hashValue;
            }
            return string.Empty;
        } 
    }
}