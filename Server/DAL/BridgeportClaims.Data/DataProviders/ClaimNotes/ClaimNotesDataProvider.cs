using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.RedisCache.Domain;
using BridgeportClaims.RedisCache.Keys;
using Dapper;
using SQLinq;
using SQLinq.Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public class ClaimNotesDataProvider : IClaimNotesDataProvider
    {
        private readonly Lazy<IRedisDomain> _redisDomain;

        public ClaimNotesDataProvider(Lazy<IRedisDomain> redisDomain)
        {
            _redisDomain = redisDomain;
        }

        public async Task<IList<KeyValuePair<int, string>>> GetClaimNoteTypesAsync()
        {
            var cacheKey = new ClaimNoteTypeCacheKey();
            var result = await _redisDomain.Value.GetAsync<IList<KeyValuePair<int, string>>>(cacheKey).ConfigureAwait(false);
            var claimNotes = result.ReturnResult;
            if (!result.Success || null == claimNotes)
            {
                claimNotes = GetClaimNoteTypesFromDb();
                if (null != claimNotes)
                {
                    await _redisDomain.Value.AddAsync(cacheKey, claimNotes, cacheKey.RedisExpirationTimespan)
                        .ConfigureAwait(false);
                }
            }
            return claimNotes;
        }

        private static IList<KeyValuePair<int, string>> GetClaimNoteTypesFromDb() => DisposableService.Using(
            () => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query(new SQLinq<ClaimNoteTypeDto>()
                        .Select(c => new {c.ClaimNoteTypeId, c.TypeName}))
                    ?.Select(s => new KeyValuePair<int, string>(s.ClaimNoteTypeId, s.TypeName))
                    .OrderBy(x => x.Value).ToList();
            });

        public void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int? noteTypeId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@NoteText", note, DbType.AnsiString);
                ps.Add("@EnteredByUserID", enteredByUserId, DbType.String);
                ps.Add("@NoteTypeID", noteTypeId, DbType.Int32);
                conn.Execute("[claims].[uspAddOrUpdateClaimNote]", ps, commandType: CommandType.StoredProcedure);
            });

        public void DeleteClaimNote(int claimId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("DECLARE @ClaimdID INT = " + claimId + "; " +
                                                             $"DELETE dbo.ClaimNote WHERE ClaimID = @ClaimdID;", conn),
                    cmd =>
                    {
                        cmd.CommandType = CommandType.Text;
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        cmd.ExecuteNonQuery();
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    });
            });
    }
}
