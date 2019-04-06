using System.Reflection;
using BridgeportClaims.Common.Disposable;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public class InvoiceProvider
    {
        public void ProcessInvoice()
        {
            DisposableService.Using(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
            {
                if (null != resourceStream)
                {
                    DisposableService.Using(() => new Document(PageSize.LETTER), doc =>
                    {
                        DisposableService.Using(() => PdfWriter.GetInstance(doc, resourceStream), writer =>
                        {
                            if (!doc.IsOpen())
                            {
                                doc.Open();
                            }

                            doc.Close();
                            writer.Close();
                            resourceStream.Close();
                        });
                    });
                }
            });
        }
    }
}
