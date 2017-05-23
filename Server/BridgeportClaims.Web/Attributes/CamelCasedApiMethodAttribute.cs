using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;

namespace BridgeportClaims.Web.Attributes
{
    public class CamelCasedApiMethodAttribute : ActionFilterAttribute
    {
        private static readonly JsonMediaTypeFormatter CamelCasingFormatter = new JsonMediaTypeFormatter();

        static CamelCasedApiMethodAttribute()
        {
            CamelCasingFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override void OnActionExecuted(HttpActionExecutedContext httpActionExecutedContext)
        {
            var objectContent = httpActionExecutedContext.Response.Content as ObjectContent;
            if (objectContent?.Formatter is JsonMediaTypeFormatter)
                httpActionExecutedContext.Response.Content = new ObjectContent(objectContent.ObjectType,
                    objectContent.Value, CamelCasingFormatter);

        }
    }
}