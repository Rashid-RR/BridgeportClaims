using NLog;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;
using System.Security.Claims;

namespace BridgeportClaims.Web.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _issuer;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            try
            {
                if (null == data)
                    throw new ArgumentNullException(nameof(data));

                var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
                var symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];
                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
                var signingKey = new HmacSigningCredentials(keyByteArray);
                var issued = data.Properties.IssuedUtc;
                var expires = data.Properties.ExpiresUtc;
                if (null == issued)
                    throw new ArgumentNullException(nameof(issued));
                if (null == expires)
                    throw new ArgumentNullException(nameof(expires));
                var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime,
                    expires.Value.UtcDateTime, signingKey);
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return new AuthenticationTicket(new ClaimsIdentity(), new AuthenticationProperties());
        }
    }
}