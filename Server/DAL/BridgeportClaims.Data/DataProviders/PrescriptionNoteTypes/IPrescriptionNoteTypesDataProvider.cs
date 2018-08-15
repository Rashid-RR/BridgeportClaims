using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public interface IPrescriptionNoteTypesDataProvider
    {
        IEnumerable<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes();
    }
}