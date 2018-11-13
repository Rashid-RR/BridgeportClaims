using System.Data;

namespace BridgeportClaims.Word.Templating
{
    public interface IWordTemplate
    {
        string TransformDocumentText(int claimId, string userId, string docText, int prescriptionId);
        string TransformDrNoteDocumentText(int claimId, string userId, string docText, int firstPrescriptionId, DataTable dt);
    }
}