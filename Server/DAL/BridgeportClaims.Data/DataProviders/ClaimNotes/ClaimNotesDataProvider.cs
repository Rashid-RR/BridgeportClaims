﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public class ClaimNotesDataProvider : IClaimNotesDataProvider
    {
        public IList<KeyValuePair<int, string>> GetClaimNoteTypes() => DisposableService.Using(
            () => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                conn.Open();
                var results = conn.Query<ClaimNoteTypeDto>(
                    "SELECT cn.ClaimNoteTypeID ClaimNoteTypeId, cn.TypeName FROM dbo.ClaimNoteType cn",
                    commandType: CommandType.Text);
                return results?.Select(s => new KeyValuePair<int, string>(s.ClaimNoteTypeId, s.TypeName))
                    .OrderBy(x => x.Value).ToList();
            });

        public void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int? noteTypeId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
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
