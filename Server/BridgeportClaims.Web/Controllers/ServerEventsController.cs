using System.Threading.Tasks;
using System.Web.Http;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/ServerEvents")]
    [Authorize(Roles = "Admin")]
    public class ServerEventsController : ApiController
    {
        public async Task<IHttpActionResult> ImportPaymentFile(int year, int month, int? day = null)
        {
            return await Task.Run(() =>
            {
                return Ok();
            });
        }
    }
}
