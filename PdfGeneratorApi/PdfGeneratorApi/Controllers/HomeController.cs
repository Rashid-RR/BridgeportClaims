using System.Web.Http;

namespace PdfGeneratorApi.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("shit")]
        public IHttpActionResult GetShit()
        {
            return Ok("FUKC");
        }
    }
}
