using System.Data;

namespace BridgeportClaims.Pdf.ITextPdfFactory
{
    public interface IPdfFactory
    {
        string GeneratePdf(DataTable dt);
    }
}