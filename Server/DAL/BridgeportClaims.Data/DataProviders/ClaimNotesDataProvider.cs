using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using FluentNHibernate.Utils;

namespace BridgeportClaims.Data.DataProviders
{
    public class ClaimNotesDataProvider : IClaimNotesDataProvider
    {
        private readonly IRepository<ClaimNote> _claimNoteRepository;
        private readonly IRepository<Claim> _claimRepository;
        private readonly IRepository<AspNetUsers> _userRepository;
        private readonly IRepository<ClaimNoteType> _claimNoteTypeRepository;

        public ClaimNotesDataProvider(IRepository<ClaimNote> claimNoteRepository, 
            IRepository<Claim> claimRepository, IRepository<AspNetUsers> userRepository, 
            IRepository<ClaimNoteType> claimNoteTypeRepository)
        {
            _claimNoteRepository = claimNoteRepository;
            _claimRepository = claimRepository;
            _userRepository = userRepository;
            _claimNoteTypeRepository = claimNoteTypeRepository;
        }

        public IList<KeyValuePair<int, string>> GetClaimNoteTypes()
        {
            var types = (from s in _claimNoteTypeRepository.GetAll()
                select new KeyValuePair<int, string>(s.ClaimNoteTypeId, s.TypeName)).ToList();
            return types;
        }

        public void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int noteTypeId)
        {
            var now = DateTime.Now;
            var claimNote = _claimNoteRepository.GetSingleOrDefault(x => x.Claim.ClaimId == claimId);
            if (null == claimNote)
            {
                claimNote = new ClaimNote
                {
                    CreatedOn = now,
                    UpdatedOn = now,
                    Claim = _claimRepository.Get(claimId),
                    AspNetUsers = _userRepository.Get(enteredByUserId),
                    ClaimNoteType = _claimNoteTypeRepository.Get(noteTypeId),
                    NoteText = note
                };
            }
            else
            {
                claimNote.NoteText = note;
                claimNote.AspNetUsers = _userRepository.Get(enteredByUserId);
                claimNote.ClaimNoteType = _claimNoteTypeRepository.Get(noteTypeId);
                claimNote.UpdatedOn = now;
            }
            _claimNoteRepository.SaveOrUpdate(claimNote);
        }
    }
}
