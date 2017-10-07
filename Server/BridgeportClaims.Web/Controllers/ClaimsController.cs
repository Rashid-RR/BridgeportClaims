using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{ 
	[Authorize(Roles = "User")]
	[RoutePrefix("api/claims")]
	public class ClaimsController : BaseApiController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IClaimsDataProvider _claimsDataProvider;

		public ClaimsController(IClaimsDataProvider claimsDataProvider)
		{
			_claimsDataProvider = claimsDataProvider;
		}

		[HttpPost]
		[Route("getclaimsdata")]
		public IHttpActionResult GetClaimsData([FromBody] ClaimsSearchViewModel model)
		{
			try
			{
				if (0 == model.ClaimId)
					throw new Exception("Error, cannot pass in a Claim ID value of zero.");
				if (null != model.ClaimId) return GetClaimsDataByClaimId(model.ClaimId.Value);

				// Search terms passed, so we're at least performing a search first to see if multiple results appear.
				var claimsData = _claimsDataProvider.GetClaimsData(model.ClaimNumber,
					model.FirstName, model.LastName, model.RxNumber, model.InvoiceNumber);
				if (null != claimsData && claimsData.Count == 1)
					return GetClaimsDataByClaimId(claimsData.Single().ClaimId);
				return Ok(claimsData);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		private IHttpActionResult GetClaimsDataByClaimId(int claimId)
		{
			return	Ok(_claimsDataProvider.GetClaimsDataByClaimId(claimId));
		}

	    [HttpPost]
	    [Route("set-flex2")]
	    public async Task<IHttpActionResult> SetClaimFlex2(int claimId, int claimFlex2Id)
	    {
	        try
	        {
	            return await Task.Run(() =>
	            {
	                var msg = string.Empty;
	                var operation = _claimsDataProvider.AddOrUpdateFlex2(claimId, claimFlex2Id);
                    switch(operation)
                    {
                        case EntityOperation.Add:
                            msg = "The claim's Flex2 was added successfully.";
                            break;
                        case EntityOperation.Update:
                            msg = "The claim's Flex2 was updated successfully.";
                            break;
                        case EntityOperation.Delete:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
	                return Ok(new {message = msg});
	            });
	        }
	        catch (Exception ex)
	        {
	            Logger.Error(ex);
	            return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
	    }
	}
}
