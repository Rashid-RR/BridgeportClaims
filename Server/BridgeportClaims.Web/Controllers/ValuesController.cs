using System;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class ValuesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        //[DeflateCompression]
        public async Task<IHttpActionResult> NativeSerializationTest()
        {
            try
            {
                return
                    await Task.Run(() =>
                    {
                        var data = new
                        {
                            FirstName = "Jordan",
                            LastName = "Gurney",
                            UrlToPost = "HttpPost"
                        };
                        return Ok(data);
                    });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
