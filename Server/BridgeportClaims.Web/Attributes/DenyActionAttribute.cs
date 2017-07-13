using System.Web.Http;
using System.Web.Http.Controllers;

namespace BridgeportClaims.Web.Attributes
{
    public class DenyActionAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            // TODO: enhance with special, allowed exceptions.
            return false;
        }
    }
}