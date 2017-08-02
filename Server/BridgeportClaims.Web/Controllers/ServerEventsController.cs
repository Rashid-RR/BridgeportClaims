using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.FilePatterns;
using BridgeportClaims.Data.DataProviders.Payments;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/ServerEvents")]
    [Authorize(Roles = "Admin")]
    public class ServerEventsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsDataProvider _paymentsDataProvider;

        public ServerEventsController(IPaymentsDataProvider paymentsDataProvider)
        {
            _paymentsDataProvider = paymentsDataProvider;
        }

        public async Task<IHttpActionResult> ImportPaymentFile(int year, int month)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var paymentFileName = FilePatternProvider.GetPaymentFilePattern(year, month);
                    _paymentsDataProvider.ImportPaymentFile(paymentFileName);
                    return Ok(new {message = "The Payment File was Processed Successfully"});
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            });
        }
    }
}