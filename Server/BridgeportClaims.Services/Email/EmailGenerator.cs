using System;
using System.Collections.Specialized;
using System.Configuration;

namespace BridgeportClaims.Services.Email
{
    public class EmailGenerator
    {
        private readonly string _environmentHostNameValue;

        public EmailGenerator(string environmentHostNameValue)
        {
            _environmentHostNameValue = environmentHostNameValue 
                ?? throw new ArgumentNullException(nameof(environmentHostNameValue));
        }

        private const string EmailUserName = "emailUserName";
        private const string EmailPassword = "emailPassword";
        private const string EmailPort = "emailPort";
        private const string EmailHostName = "emailHostName";
        private const string EmailEnableSsl = "emailEnableSsl";
        private const string EnvironmentHostName = "environmentHostName";

        public NameValueCollection GetParametersForEmailVariables()
        {
            var coll = new NameValueCollection
            {
                {EmailUserName, ConfigurationManager.AppSettings[EmailUserName]},
                {EmailPassword, ConfigurationManager.AppSettings[EmailPassword]},
                {EmailPort, ConfigurationManager.AppSettings[EmailPort]},
                {EmailHostName, ConfigurationManager.AppSettings[EmailHostName]},
                {EmailEnableSsl, ConfigurationManager.AppSettings[EmailEnableSsl]},
                {EnvironmentHostName, _environmentHostNameValue}
            };
            return coll;
        }
    }
}