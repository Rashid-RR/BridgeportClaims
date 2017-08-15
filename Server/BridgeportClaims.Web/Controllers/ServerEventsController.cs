using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ImportFile;
using BridgeportClaims.Data.DataProviders.Payments;

namespace BridgeportClaims.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	[RoutePrefix("api/ServerEvents")]
	public class ServerEventsController : BaseApiController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IPaymentsDataProvider _paymentsDataProvider;
	    private readonly IImportFileProvider _importFileProvider;

		public ServerEventsController(IPaymentsDataProvider paymentsDataProvider, IImportFileProvider importFileProvider)
		{
		    _paymentsDataProvider = paymentsDataProvider;
		    _importFileProvider = importFileProvider;
		}

		[HttpPost]
		[Route("ImportPaymentFile")]
		public async Task<IHttpActionResult> ImportPaymentFile(string fileName)
		{
			return await Task.Run(() =>
			{
				try
				{
					_paymentsDataProvider.ImportPaymentFile(fileName);
				    _importFileProvider.MarkFileProcessed(fileName);

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