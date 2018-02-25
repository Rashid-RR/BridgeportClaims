using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace BridgeportClaims.Pdf.ITextPdfFactory
{
    public class PdfFactory : IPdfFactory
    {
        public string GeneratePdf(DataTable dt)
        {
            const string fileName = "Test.pdf";
            var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
            ExportDataTableToPdf(dt, fullFilePath, "Friendly Name");
            return "";
        }

        void ExportDataTableToPdf(DataTable dt, string pdfPath, string header)
        {
            using (var fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    doc.SetPageSize(PageSize.A4);
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        if (!doc.IsOpen())
                            doc.Open();
                        
                        // Report Header.
                        BaseFont btntHead = BaseFont.CreateFont()
                    }
                }
            }
        }
    }
}