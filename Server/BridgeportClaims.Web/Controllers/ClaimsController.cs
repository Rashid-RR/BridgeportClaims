﻿using NLog;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
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
		private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
		private readonly Lazy<IClaimsDataProvider> _claimsDataProvider;
        private readonly Lazy<IClaimSearchProvider> _claimSearchProvider;
		private readonly Lazy<IClaimsEditProvider> _claimsEditProvider;

        public ClaimsController(Lazy<IClaimsDataProvider> claimsDataProvider, Lazy<IClaimsEditProvider> claimsEditProvider, Lazy<IClaimSearchProvider> claimSearchProvider)
		{
			_claimsDataProvider = claimsDataProvider;
			_claimsEditProvider = claimsEditProvider;
            _claimSearchProvider = claimSearchProvider;
        }

        [HttpPost]
        [Route("global-search")]
        public IHttpActionResult GetGlobalSearchForClaims(string searchTerm, SearchType searchType)
        {
            try
            {
                switch (searchType)
                {
                    case SearchType.ClaimNumber: // Search term provided
                        searchType = SearchType.ClaimNumber;
                        break;
                    case SearchType.LastName:
                        searchType = SearchType.LastName;
                        break;
                    case SearchType.FirstName:
                        searchType = SearchType.FirstName;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var data = _claimSearchProvider.Value.GetSearchClaimResults(searchTerm, searchType); 
                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update-claim-attorney-managed")]
        public IHttpActionResult UpdateClaimAttorneyManaged(int claimId, bool isAttorneyManaged)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _claimsEditProvider.Value.UpdateClaimAttorneyManaged(claimId, isAttorneyManaged, userId);
                return Ok(new { message = "Attorney managed was updated successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

	    [HttpPost]
	    [Route("outstanding")]
	    public IHttpActionResult GetOutstanding(ClaimSortViewModel model)
	    {
	        try
	        {
	            var outstanding = _claimsDataProvider.Value.GetOutstanding(model.ClaimId, model.Page, model.PageSize, model.Sort,
	                model.SortDirection);
	            return Ok(new {outstanding.TotalOutstandingAmount, outstanding.TotalRows, outstanding.Results});
	        }
	        catch (Exception ex)
	        {
	            Logger.Value.Error(ex);
	            return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
	    }

		[HttpPost]
		[Route("sort-episodes")]
		public IHttpActionResult SortEpisodes(SortEpisodeModel model)
		{
		    try
		    {
		        var results =
		            _claimsDataProvider.Value.GetEpisodesBlade(model.ClaimId, model.SortColumn, model.SortDirection);
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
                var modifiedByUserId = User?.Identity?.GetUserId();
                var ofBirth = model.DateOfBirth;
                var ofBirthParam = null == ofBirth ? (DateTime?) null
                    : "NULL" == ofBirth ? new DateTime(1901, 1, 1)
                    : DateTime.TryParse(ofBirth, out var dt) ? dt
                    : throw new Exception($"Could not parse Date Time value {ofBirth}");
                var genderId = model.GenderId;
		        var payorId = model.PayorId;
		        var adjustorId = model.AdjustorId;
                var attorneyId = model.AttorneyId;
		        var ofInjury = model.DateOfInjury;
                var ofInjuryParam = null == ofInjury ? (DateTime?) null
                    : "NULL" == ofInjury ? new DateTime(1901, 1, 1)
                    : DateTime.TryParse(ofInjury, out DateTime dat) ? dat
                    : throw new Exception($"Could not parse Date Time value {ofInjury}");
		        var address1 = model.Address1;
		        var address2 = model.Address2;
		        var city = model.City;
		        var stateId = model.StateId;
		        var postalCode = model.PostalCode;
		        var claimFlex2Id = model.ClaimFlex2Id;
                if (null == modifiedByUserId)
                {
                    throw new Exception("Could not locate the authenticated user.");
                }
                _claimsEditProvider.Value.EditClaim(claimId, modifiedByUserId, ofBirthParam, genderId, payorId,
		            adjustorId,attorneyId, ofInjuryParam, address1, address2, city, stateId, postalCode, claimFlex2Id);
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
				if (default (int) == model.ClaimId)
					throw new Exception("Error, cannot pass in a Claim ID value of zero.");
			    if (null != model.ClaimId)
			    {
			        return GetClaimsDataByClaimId(model.ClaimId.Value);
			    }
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
	    [Route("set-max-balance")]
	    public IHttpActionResult SetMaxBalance(int claimId, bool isMaxBalance)
	    {
	        try
	        {
	            var userId = User.Identity.GetUserId();
	            if (null == userId)
	                throw new Exception("Error, could not find the logged in user.");
	            var msg = _claimsDataProvider.Value.UpdateIsMaxBalance(claimId, isMaxBalance, userId);
	            return Ok(new {message = msg});
	        }
	        catch (Exception ex)
	        {
	            Logger.Value.Error(ex);
	            return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
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
