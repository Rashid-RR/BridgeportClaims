using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BridgeportClaims.Web.Infrastructure
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>();
            var daysInWork = (DateTime.Now.Date - user.RegisteredDate).TotalDays;
            if (daysInWork > 90)
            {
                claims.Add(CreateClaim("FTE", "1"));
            }
            else
            {
                claims.Add(CreateClaim("FTE", "0"));
            }
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}