using System.Data;

namespace BridgeportClaims.Pdf.Factories
{
    public interface IPdfFactory
    {
        string GeneratePdf(DataTable dt);
    }
}