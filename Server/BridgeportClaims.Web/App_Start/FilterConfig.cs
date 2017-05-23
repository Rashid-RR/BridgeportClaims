using BridgeportClaims.Web.ErrorHandler;
using System.Web.Mvc;

namespace BridgeportClaims.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
