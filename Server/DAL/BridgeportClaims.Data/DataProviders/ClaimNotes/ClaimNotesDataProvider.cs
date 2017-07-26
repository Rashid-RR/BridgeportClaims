using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public class ClaimNotesDataProvider : IClaimNotesDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly IRepository<ClaimNoteType> _claimNoteTypeRepository;
        private readonly IMemoryCacher _memoryCacher;

        public ClaimNotesDataProvider(IStoredProcedureExecutor storedProcedureExecutor, 
            IRepository<ClaimNoteType> claimNoteTypeRepositor, IMemoryCacher memoryCacher)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
            _claimNoteTypeRepository = claimNoteTypeRepositor;
            _memoryCacher = memoryCacher;
        }

        public IList<KeyValuePair<int, string>> GetClaimNoteTypes()
        {
            var result = _memoryCacher.GetValue(c.ClaimNoteTypesKey) as IList<KeyValuePair<int, string>>;
            if (null != result)
                return result;
            result = _claimNoteTypeRepository.GetAll()
                .Select(s => new KeyValuePair<int, string>(s.ClaimNoteTypeId, s.TypeName)).ToList();
            _memoryCacher.Add(c.ClaimNoteTypesKey, result, DateTimeOffset.UtcNow.AddDays(1));
            return result;
        }

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
    }
}
