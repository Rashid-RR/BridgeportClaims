using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.Infrastructure;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/auto-upload")]
    public class AutoUploadController : BaseApiController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return Content(HttpStatusCode.UnsupportedMediaType, "Unsupported media type.");

            // Read the file and form data.
            var provider = new MultipartFormDataMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            // Extract the fields from the form data.
            var description = provider.FormData["description"];
        }
    }
}
