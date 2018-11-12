using System.Collections.Generic;
using BridgeportClaims.Word.Enums;

namespace BridgeportClaims.Word.FileDriver
{
    public interface IWordFileDriver
    {
        string GetLetterByType(int claimId, string userId, LetterType type, int? prescriptionId = null);
        string GetDrLetter(int claimId, int firstPrescriptionId, IEnumerable<int> prescriptionIds, string userId);
    }
}