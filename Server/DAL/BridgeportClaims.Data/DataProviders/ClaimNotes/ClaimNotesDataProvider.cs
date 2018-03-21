using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public class ClaimNotesDataProvider : IClaimNotesDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly IRepository<ClaimNoteType> _claimNoteTypeRepository;

        public ClaimNotesDataProvider(IStoredProcedureExecutor storedProcedureExecutor, 
            IRepository<ClaimNoteType> claimNoteTypeRepositor)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
            _claimNoteTypeRepository = claimNoteTypeRepositor;
        }

        public IList<KeyValuePair<int, string>> GetClaimNoteTypes() => _claimNoteTypeRepository.GetAll()
            .Select(s => new KeyValuePair<int, string>(s.ClaimNoteTypeId, s.TypeName)).OrderBy(x => x.Value).ToList();

        public void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int? noteTypeId)
        {
            var listToAdd = new List<SqlParameter>();
            var claimIdParam = new SqlParameter
            {
                DbType = DbType.Int32,
                Value = claimId,
                ParameterName = "ClaimID"
            };
            var noteParam = new SqlParameter
            {
                DbType = DbType.String,
                Value = note,
                ParameterName = "NoteText"
            };
            var enteredByUserIdParam = new SqlParameter
            {
                DbType = DbType.String,
                Value = enteredByUserId,
                ParameterName = "EnteredByUserID"
            };
            var noteTypeIdParam = new SqlParameter
            {
                DbType = DbType.Int32,
                Value = noteTypeId,
                ParameterName = "NoteTypeID"
            };
            listToAdd.Add(noteTypeIdParam);
            listToAdd.Add(claimIdParam);
            listToAdd.Add(noteParam);
            listToAdd.Add(enteredByUserIdParam);
            _storedProcedureExecutor.ExecuteNoResultStoredProcedure("EXEC dbo.uspAddOrUpdateClaimNote @ClaimID = :ClaimID, " +
                    "@NoteText = :NoteText, @EnteredByUserID = :EnteredByUserID, @NoteTypeID = :NoteTypeID", listToAdd);
        }

        public void DeleteClaimNote(int claimId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("DECLARE @ClaimdID INT = " + claimId + "; " +
                                                             $"DELETE dbo.ClaimNote WHERE ClaimID = {claimId}", conn),
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
