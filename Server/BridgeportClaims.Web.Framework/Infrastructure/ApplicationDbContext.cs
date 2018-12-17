using BridgeportClaims.Common.Constants;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Framework.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base(StringConstants.DbConnStrName, false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}