using System;
using System.IdentityModel.Tokens;
using BridgeportClaims.Common.Config;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;

namespace BridgeportClaims.Web.Providers
{
    public class BridgeportClaimsJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer;

        public BridgeportClaimsJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            var audienceId = ConfigService.GetAppSetting("as:AudienceId");
            var symmetricKeyAsBase64 = ConfigService.GetAppSetting("as:AudienceSecret");
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

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}