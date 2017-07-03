using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public class PrescriptionNoteTypesDataProvider : IPrescriptionNoteTypesDataProvider
    {
        private readonly IRepository<PrescriptionNoteType> _repository;

        public PrescriptionNoteTypesDataProvider(IRepository<PrescriptionNoteType> repository)
        {
            _repository = repository;
        }

        public IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes()
        {
            var query = _repository.GetAll()
                .Select(c => new PrescriptionNoteTypesDto
                {
                    PrescriptionNoteTypeId = c.PrescriptionNoteTypeId,
                    TypeName = c.TypeName
                });
            return query.ToList();
        }
    }
}
