using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public class PrescriptionNoteTypesDataProvider : IPrescriptionNoteTypesDataProvider
    {
        private readonly IRepository<PrescriptionNoteType> _repository;
        private readonly IMemoryCacher _memoryCacher;

        public PrescriptionNoteTypesDataProvider(IRepository<PrescriptionNoteType> repository, IMemoryCacher memoryCacher)
        {
            _repository = repository;
            _memoryCacher = memoryCacher;
        }

        public IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes()
        {
            var result = _memoryCacher.AddOrGetExisting(c.PrescriptionNoteTypesKey, () =>
            {
                var query = _repository.GetAll()
                    .Select(c => new PrescriptionNoteTypesDto
                    {
                        PrescriptionNoteTypeId = c.PrescriptionNoteTypeId,
                        TypeName = c.TypeName
                    });
                return query.ToList();
            });
            return result;
        }
    }
}
