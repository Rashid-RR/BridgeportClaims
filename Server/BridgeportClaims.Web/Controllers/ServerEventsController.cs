using NLog;
using System;
using System.Web.Http;
using BridgeportClaims.Business.Payments;
using BridgeportClaims.Data.DataProviders.ImportFiles;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/ServerEvents")]
    public class ServerEventsController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPaymentsBusiness> _paymentsBusiness;
        private readonly Lazy<IImportFileProvider> _importFileProvider;

        public ServerEventsController(Lazy<IPaymentsBusiness> paymentsBusiness,
            Lazy<IImportFileProvider> importFileProvider)
        {
            _paymentsBusiness = paymentsBusiness;
            _importFileProvider = importFileProvider;
        }

        [HttpPost]
        [Route("ImportPaymentFile")]
        public IHttpActionResult ImportPaymentFile(string fileName)
        {
            try
            {
                _paymentsBusiness.Value.ImportPaymentFile(fileName);
                _importFileProvider.Value.MarkFileProcessed(fileName);
                return Ok(new {message = "The Payment File was Processed Successfully"});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }

        }
    }
}