using System;
using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace BridgeportClaims.Pdf.ITextPdfFactory
{
    public class PdfFactory
    {
        public void GeneratePdf(DataTable dt)
        {
            var exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var exportFile = Path.Combine(exportFolder, "Test.pdf");
            DisposableService.Using(() => new PdfWriter(exportFile), writer =>
            {
                DisposableService.Using(() => new PdfDocument(writer), pdf =>
                {
                    DisposableService.Using(() => new Document(pdf), doc =>
                    {
                        doc.Add(new Paragraph("Yo Yo Yo"));
                    });
                });
            });
        }
    }
}