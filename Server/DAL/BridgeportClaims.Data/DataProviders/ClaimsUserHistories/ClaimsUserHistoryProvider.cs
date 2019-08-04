using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.RedisCache.Clearing;
using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.RedisCache.Keys;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimsUserHistories
{
    public class ClaimsUserHistoryProvider : IClaimsUserHistoryProvider
    {
        private readonly Lazy<IRedisDomain> _redisDomain;
        private readonly Lazy<ICachingClearingService> _clearingProvider;

        public ClaimsUserHistoryProvider(Lazy<IRedisDomain> redisDomain,
            Lazy<ICachingClearingService> clearingProvider)
        {
            _redisDomain = redisDomain;
            _clearingProvider = clearingProvider;
        }

        public async Task<IList<ClaimsUserHistoryDto>> GetClaimsUserHistoryAsync(string userId)
        {
            var cacheKey = new ClaimUserHistoryCacheKey(userId);
            var result = await _redisDomain.Value.GetAsync<IList<ClaimsUserHistoryDto>>
                (cacheKey).ConfigureAwait(false);
            var history = result.ReturnResult;
            if (!result.Success || null == history)
            {
                history = GetClaimsUserHistoryFromDb(userId)?.ToList();
                if (null != history)
                {
                    await _redisDomain.Value.AddAsync(cacheKey, history, cacheKey.RedisExpirationTimespan)
                        .ConfigureAwait(false);
                }
            }
            return history;
        }

        private IEnumerable<ClaimsUserHistoryDto> GetClaimsUserHistoryFromDb(string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var maxClaimsLookup = int.TryParse(cs.GetAppSetting(StringConstants.MaxClaimsLookupHistoryItemsKey), out var i) ? i : 22;
                return DisposableService.Using(() => new SqlCommand("dbo.uspGetClaimUserHistory", conn), cmd =>
                {
                    IList<ClaimsUserHistoryDto> retVal = new List<ClaimsUserHistoryDto>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Direction = ParameterDirection.Input;
                    userIdParam.DbType = DbType.String;
                    userIdParam.Size = 128;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.Value = userId ?? (object) DBNull.Value;
                    cmd.Parameters.Add(userIdParam);
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
                                ClaimId = !reader.IsDBNull(claimIdOrdinal) ? reader.GetInt32(claimIdOrdinal) : default,
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
                    {
                        conn.Close();
                    }
                    return retVal.OrderByDescending(x => x.CreatedOnUtc).Take(maxClaimsLookup);
                });
            });

        public async Task InsertClaimsUserHistoryAsync(string userId, int claimId)
        {
            await _clearingProvider.Value.ClearClaimUserHistoryCache(userId).ConfigureAwait(false);
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "dbo.uspInsertClaimsUserHistory";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new { ClaimID = claimId, UserID = userId }, commandType: CommandType.StoredProcedure);
            });
        }
    }
}