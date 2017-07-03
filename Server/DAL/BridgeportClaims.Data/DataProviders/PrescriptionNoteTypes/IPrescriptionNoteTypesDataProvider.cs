using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public interface IPrescriptionNoteTypesDataProvider
    {
        IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes();
    }
}