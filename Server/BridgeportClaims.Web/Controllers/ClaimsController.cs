using NLog;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
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
		private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
		private readonly Lazy<IClaimsDataProvider> _claimsDataProvider;
		private readonly Lazy<IClaimsEditProvider> _claimsEditProvider;

		public ClaimsController(Lazy<IClaimsDataProvider> claimsDataProvider, Lazy<IClaimsEditProvider> claimsEditProvider)
		{
			_claimsDataProvider = claimsDataProvider;
			_claimsEditProvider = claimsEditProvider;
		}

		[HttpPost]
		[Route("sort-episodes")]
		public IHttpActionResult SortEpisodes(SortEpisodeModel model)
		{
		    try
		    {
		        var userId = User.Identity.GetUserId();
		        if (null == userId)
		            throw new Exception("Error, could not find the logged in user.");
		        var results =
		            _claimsDataProvider.Value.GetEpisodesBlade(model.ClaimId, model.SortColumn, model.SortDirection, userId);
		        return Ok(results);
		    }
		    catch (Exception ex)
		    {
		        Logger.Value.Error(ex);
		        return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
		    }
		}

		[HttpPost]
		[Route("edit-claim")]
		public IHttpActionResult EditClaim(ClaimEditModel model)
		{
		    try
		    {
		        var claimId = model.ClaimId;
		        var dateOfBirth = model.DateOfBirth;
		        var genderId = model.GenderId;
		        var payorId = model.PayorId;
		        var adjustorId = model.AdjustorId;
		        var adjustorPhone = model.AdjustorPhone;
		        var dateOfInjury = model.DateOfInjury;
		        var adjustorFax = model.AdjustorFax;
		        var address1 = model.Address1;
		        var address2 = model.Address2;
		        var city = model.City;
		        var stateId = model.StateId;
		        var postalCode = model.PostalCode;
		        var claimFlex2Id = model.ClaimFlex2Id;
		        var userId = User?.Identity?.GetUserId();
		        var ext = model.AdjustorExtension;
		        if (null == userId)
		            throw new Exception("Could not locate the authenticated user.");
		        _claimsEditProvider.Value.EditClaim(claimId, userId, null == dateOfBirth ? (DateTime?) null
		            : "NULL" == dateOfBirth ? new DateTime(1901, 1, 1)
		            : DateTime.TryParse(dateOfBirth, out DateTime dt) ? dt
		            : throw new Exception($"Could not parse Date Time value {dateOfBirth}"), genderId, payorId,
		            adjustorId, adjustorPhone, null == dateOfInjury ? (DateTime?) null
		            : "NULL" == dateOfInjury ? new DateTime(1901, 1, 1)
		            : DateTime.TryParse(dateOfInjury, out DateTime dat) ? dat
		            : throw new Exception($"Could not parse Date Time value {dateOfInjury}"), adjustorFax,
		            address1, address2, city, stateId, postalCode, claimFlex2Id, ext);
		        return Ok(new {message = "The claim was updated successfully."});
		    }
		    catch (Exception ex)
		    {
		        Logger.Value.Error(ex);
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
				var claimsData = _claimsDataProvider.Value.GetClaimsData(model.ClaimNumber,
					model.FirstName, model.LastName, model.RxNumber, model.InvoiceNumber);
				if (null != claimsData && claimsData.Count == 1)
					return GetClaimsDataByClaimId(claimsData.Single().ClaimId);
				return Ok(claimsData);
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
			}
		}

		private IHttpActionResult GetClaimsDataByClaimId(int claimId)
		{
			var userId = User.Identity.GetUserId();
			if (null == userId)
				throw new Exception("Error, could not find the logged in user.");
			var results = _claimsDataProvider.Value.GetClaimsDataByClaimId(claimId, userId);
			return Ok(results);
		}


		[HttpPost]
		[Route("set-flex2")]
		public IHttpActionResult SetClaimFlex2(int claimId, int claimFlex2Id)
		{
		    try
		    {
		        var userId = User.Identity.GetUserId();
		        if (null == userId)
		            throw new Exception("Error, could not find the logged in user.");
		        var msg = string.Empty;
		        var operation = _claimsDataProvider.Value.AddOrUpdateFlex2(claimId, claimFlex2Id, userId);
		        switch (operation)
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
		    }
		    catch (Exception ex)
		    {
		        Logger.Value.Error(ex);
		        return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
		    }
		}
	}
}
