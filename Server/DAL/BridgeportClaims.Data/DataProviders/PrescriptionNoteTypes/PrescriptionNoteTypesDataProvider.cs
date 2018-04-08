using System;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public class PrescriptionNoteTypesDataProvider : IPrescriptionNoteTypesDataProvider
    {
        private readonly Lazy<IRepository<PrescriptionNoteType>> _repository;

        public PrescriptionNoteTypesDataProvider(Lazy<IRepository<PrescriptionNoteType>> repository)
        {
            _repository = repository;
        }

        public IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes() => _repository.Value.GetAll()
            .Select(c => new PrescriptionNoteTypesDto
            {
                PrescriptionNoteTypeId = c.PrescriptionNoteTypeId,
                TypeName = c.TypeName
            }).OrderBy(x => x.TypeName).ToList();
    }
}
