using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimsUserHistories
{
    public class ClaimsUserHistoryProvider : IClaimsUserHistoryProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly IRepository<VwClaimInfo> _vwClaimInfoRepository;
        private readonly IRepository<ClaimsUserHistory> _claimsUserHistoryRepository;

        public ClaimsUserHistoryProvider(IStoredProcedureExecutor storedProcedureExecutor, 
            IRepository<VwClaimInfo> vwClaimInfoRepository, 
            IRepository<ClaimsUserHistory> claimsUserHistoryRepository)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
            _vwClaimInfoRepository = vwClaimInfoRepository;
            _claimsUserHistoryRepository = claimsUserHistoryRepository;
        }

        public IList<ClaimsUserHistoryDto> GetClaimsUserHistory(string userId)
        {
            var maxClaimsLookup = Convert.ToInt32(cs.GetAppSetting(c.MaxClaimsLookupHistoryItemsKey));
            var claimsUserHistories = _claimsUserHistoryRepository.GetMany(x => x.AspNetUsers.Id == userId)
                .Join(_vwClaimInfoRepository.GetAll(), h => h.Claim.ClaimId, c => c.ClaimId, (h, c) => new {h, c})
                .OrderByDescending(w => w.h.CreatedOnUtc)
                .Select(s => new ClaimsUserHistoryDto
                {
                    ClaimId = s.c.ClaimId,
                    ClaimNumber = s.c.ClaimNumber,
                    Name = s.c.Name,
                    InjuryDate = s.c.InjuryDate,
                    Carrier = s.c.Carrier
                }).Take(maxClaimsLookup).ToList();
            return claimsUserHistories;
        }

        public void InsertClaimsUserHistory(string userId, int claimId)
        {
            var claimIdParam = new SqlParameter
            {
                Value = claimId,
                ParameterName = "ClaimID",
                DbType = DbType.Int32
            };
            var userIdParam = new SqlParameter
            {
                Value = userId,
                ParameterName = "UserID",
                DbType = DbType.String
            };
            _storedProcedureExecutor.ExecuteNoResultStoredProcedure(
                "EXEC dbo.uspInsertClaimsUserHistory @ClaimID = :ClaimID, @UserID = :UserID",
                new List<SqlParameter> {claimIdParam, userIdParam});
        }
    }
}