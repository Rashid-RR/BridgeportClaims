using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.DomainModels.Views;
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

        public IList<ClaimsUserHistoryDto> GetClaimsUserHistory(string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var maxClaimsLookup = int.TryParse(cs.GetAppSetting(c.MaxClaimsLookupHistoryItemsKey), out int i) ? i : 22;
                return DisposableService.Using(() => new SqlCommand("dbo.uspGetClaimUserHistory", conn), cmd =>
                {
                    IList<ClaimsUserHistoryDto> retVal = new List<ClaimsUserHistoryDto>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Direction = ParameterDirection.Input;
                    userIdParam.DbType = DbType.StringFixedLength;
                    userIdParam.Size = 128;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.Value = userId ?? (object) DBNull.Value;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var nameOrdinal = reader.GetOrdinal("Name");
                        var injuryDateOrdinal = reader.GetOrdinal("InjuryDate");
                        var carrierOrdinal = reader.GetOrdinal("Carrier");
                        var createdOnUtcOrdinal = reader.GetOrdinal("CreatedOnUtc");
                        while (reader.Read())
                        {
                            var injuryDate = !reader.IsDBNull(injuryDateOrdinal) ? reader.GetDateTime(injuryDateOrdinal) : (DateTime?)null;
                            var result = new ClaimsUserHistoryDto
                            {
                                ClaimId = !reader.IsDBNull(claimIdOrdinal) ? reader.GetInt32(claimIdOrdinal) : default(int),
                                Carrier = !reader.IsDBNull(carrierOrdinal) ? reader.GetString(carrierOrdinal) : string.Empty,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : string.Empty,
                                InjuryDate = injuryDate?.ToString("M/d/yyyy"),
                                Name = !reader.IsDBNull(nameOrdinal) ? reader.GetString(nameOrdinal) : string.Empty,
                                CreatedOnUtc = !reader.IsDBNull(createdOnUtcOrdinal) ? reader.GetDateTime(createdOnUtcOrdinal) : DateTime.UtcNow
                            };
                            retVal.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal.OrderByDescending(x => x.CreatedOnUtc).Take(maxClaimsLookup).ToList();
                });
            });
        /*
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
        */

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