namespace BridgeportClaims.Word.Templating
{
    public interface IWordTemplater
    {
        string TransformDocumentText(int claimId, string userId, string docText, int prescriptionId);
    }
}