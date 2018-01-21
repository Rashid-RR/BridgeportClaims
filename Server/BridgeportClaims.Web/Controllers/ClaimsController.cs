using NLog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.ClaimsEdit;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{ 
	[Authorize(Roles = "User")]
	[RoutePrefix("api/claims")]
	public class ClaimsController : BaseApiController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IClaimsDataProvider _claimsDataProvider;
	    private readonly IClaimsEditProvider _claimsEditProvider;

        public ClaimsController(IClaimsDataProvider claimsDataProvider, IClaimsEditProvider claimsEditProvider)
        {
            _claimsDataProvider = claimsDataProvider;
            _claimsEditProvider = claimsEditProvider;
        }

	    [HttpPost]
	    [Route("edit-claim")]
	    public async Task<IHttpActionResult> EditClaim([FromBody] int claimId, string modifiedByUserId,
	        string dateOfBirth = "NULL", int genderId = -1, int payorId = -1, int? adjustorId = -1, string adjustorPhone = "NULL",
	        string dateOfInjury = "NULL", string adjustorFax = "NULL")
	    {
	        try
	        {
	            return await Task.Run(() =>
	            {
                    if (modifiedByUserId.IsNullOrWhiteSpace())
                        throw new ArgumentNullException(nameof(modifiedByUserId));
	                var userId = User?.Identity?.GetUserId();
	                if (null == userId)
	                    throw new Exception("Could not locate the authenticated user.");
	                _claimsEditProvider.EditClaim(claimId, userId, null == dateOfBirth ? (DateTime?) null : "NULL" == dateOfBirth ? new DateTime(1901, 1, 1)
	                        : DateTime.TryParse(dateOfBirth, out DateTime dt) ? dt : throw new Exception($"Could not parse Date Time value {dateOfBirth}"), genderId, payorId,
	                        adjustorId, adjustorPhone, null == dateOfInjury ? (DateTime?) null : "NULL" == dateOfInjury ? new DateTime(1901, 1, 1)
	                        : DateTime.TryParse(dateOfInjury, out DateTime dat) ? dat : throw new Exception($"Could not parse Date Time value {dateOfInjury}"), adjustorFax);
	                return Ok(new {message = "The claim was updated successfully."});
	            });
	        }
	        catch (Exception ex)
	        {
	            Logger.Error(ex);
	            return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
	        }
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
