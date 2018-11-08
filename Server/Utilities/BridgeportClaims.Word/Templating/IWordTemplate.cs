namespace BridgeportClaims.Word.Templating
{
    public interface IWordTemplate
    {
        string TransformDocumentText(int claimId, string userId, string docText, int? prescriptionId = null);
    }
}