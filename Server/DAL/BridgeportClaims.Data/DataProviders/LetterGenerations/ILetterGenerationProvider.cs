using System.Data;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.LetterGenerations
{
    public interface ILetterGenerationProvider
    {
        LetterGenerationDto GetLetterGenerationData(int claimId, string userId, int? prescriptionId = null);
        DrNoteLetterGenerationDto GetDrNoteLetterGenerationData(int claimId, string userId, int firstPrescriptionId, DataTable dt);
    }
}