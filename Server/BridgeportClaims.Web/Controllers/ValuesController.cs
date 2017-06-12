using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.Email;
using BridgeportClaims.Web.Email.EmailTemplateProviders.WelcomeActivationTemplate;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly IEmailService _emailService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ValuesController(IEmailService emailService)
        {
            _emailService = emailService;
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
                        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                        _emailService.SendEmail<EmailWelcomeActivationTemplate>("jordangurney@gmail.com", baseUrl);
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
