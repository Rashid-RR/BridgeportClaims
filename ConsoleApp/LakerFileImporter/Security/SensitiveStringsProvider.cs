using System.Security;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.Security
{
    public class SensitiveStringsProvider
    {
        public SecureString GetDbConnString() => Encryptor.DecryptString(cs.GetDbConnStr());
        public SecureString GetAuthenticatedPassword() => Encryptor.DecryptString(cs.GetAppSetting(c.AuthenticationPasswordKey));

        public string EncryptAuthenticatedPassword() => Encryptor.EncryptString(
            Encryptor.ToSecureString(cs.GetAppSetting(c.AuthenticationPasswordKey)));

        public string EncryptDbConnString() => Encryptor.EncryptString(Encryptor.ToSecureString(cs.GetDbConnStr()));
    }
}