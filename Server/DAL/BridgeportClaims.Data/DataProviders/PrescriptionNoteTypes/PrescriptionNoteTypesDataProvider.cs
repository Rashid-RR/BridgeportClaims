using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = _memoryCacher.GetValue(c.PrescriptionNoteTypesKey) as IList<PrescriptionNoteTypesDto>;
            if (null != result)
                return result;
            var query = _repository.GetAll()
                .Select(c => new PrescriptionNoteTypesDto
                {
                    PrescriptionNoteTypeId = c.PrescriptionNoteTypeId,
                    TypeName = c.TypeName
                });
            result = query.ToList();
            _memoryCacher.Add(c.PrescriptionNoteTypesKey, result, DateTimeOffset.UtcNow.AddDays(1));
            return result;
        }
    }
}
