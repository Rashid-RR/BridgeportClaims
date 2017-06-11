using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using NLog;
using ServiceStack.Text;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ValuesController(IDbccUserOptionsProvider dbccUserOptionsProvider)
        {
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
        }

        [HttpGet]
        //[DeflateCompression]
        public async Task<IHttpActionResult> JsonSerializerStringTest()
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
                        var json = JsonSerializer.SerializeToString(data);
                        return Ok(json);
                    });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }


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

        [HttpGet]
        //[DeflateCompression]
        public async Task<HttpResponseMessage> HttpResponseMessageStringContentTest()
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
                        var jsonString = JsonSerializer.SerializeToString(data);
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        return response;
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
