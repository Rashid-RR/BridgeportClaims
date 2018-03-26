using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Data.DataProviders.ImportFiles;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/ServerEvents")]
    public class ServerEventsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsBusiness _paymentsBusiness;
        private readonly IImportFileProvider _importFileProvider;

        public ServerEventsController(IPaymentsBusiness paymentsBusiness,
            IImportFileProvider importFileProvider)
        {
            _paymentsBusiness = paymentsBusiness;
            _importFileProvider = importFileProvider;
        }

        [HttpPost]
        [Route("ImportPaymentFile")]
        public async Task<IHttpActionResult> ImportPaymentFile(string fileName)
        {

            try
            {
                return await Task.Run(() =>
                {
                    _paymentsBusiness.ImportPaymentFile(fileName);
                    _importFileProvider.MarkFileProcessed(fileName);
                    return Ok(new {message = "The Payment File was Processed Successfully"});
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