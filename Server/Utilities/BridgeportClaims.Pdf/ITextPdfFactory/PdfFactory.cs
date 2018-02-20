using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace BridgeportClaims.Pdf.ITextPdfFactory
{
    public class PdfFactory : IPdfFactory
    {
        public string GeneratePdf(DataTable dt)
        {
            const string fileName = "Test.pdf";
            var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
            return DisposableService.Using(() => new PdfWriter(fullFilePath), writer =>
            {
                return DisposableService.Using(() => new PdfDocument(writer), pdf =>
                {
                    return DisposableService.Using(() => new Document(pdf), doc =>
                    {
                        doc.Add(new Paragraph("Yo Yo Yo"));
                        return fullFilePath;
                    });
                });
            });
        }
    }
}