using BridgeportClaims.Common.StringConstants;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BridgeportClaims.Web.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base(Constants.DbConnStrName, false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}