using System.Collections.Specialized;
using System.Configuration;

namespace BridgeportClaims.Data.Services.Emailing
{
    public class EmailGenerator
    {
        private const string EmailUserName = "emailUserName";
        private const string EmailPassword = "emailPassword";
        private const string EmailPort = "emailPort";
        private const string EmailHostName = "emailHostName";
        private const string EmailEnableSsl = "emailEnableSsl";
        private const string EnvironmentHostName = "EnvironmentHostname";
        private const string UnapprovedCodesDestinationEmailAddress = "UnapprovedCodesDestinationEmailAddress";

        public NameValueCollection GetParametersForEmailVariables()
        {
            var coll = new NameValueCollection
            {
                {EmailUserName, ConfigurationManager.AppSettings[EmailUserName]},
                {EmailPassword, ConfigurationManager.AppSettings[EmailPassword]},
                {EmailPort, ConfigurationManager.AppSettings[EmailPort]},
                {EmailHostName, ConfigurationManager.AppSettings[EmailHostName]},
                {EmailEnableSsl, ConfigurationManager.AppSettings[EmailEnableSsl]},
                {EnvironmentHostName, ConfigurationManager.AppSettings[EnvironmentHostName]},
                {UnapprovedCodesDestinationEmailAddress, ConfigurationManager.AppSettings[UnapprovedCodesDestinationEmailAddress]}
            };
            return coll;
        }
    }
}