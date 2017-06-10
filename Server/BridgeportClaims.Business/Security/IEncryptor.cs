namespace BridgeportClaims.Business.Security
{
    public interface IEncryptor
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
        string EncryptionKey { get; set; }
    }
}