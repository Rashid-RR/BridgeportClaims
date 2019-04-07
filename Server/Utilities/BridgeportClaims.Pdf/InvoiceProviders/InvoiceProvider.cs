using System;
using System.IO;
using System.Reflection;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public class InvoiceProvider : IInvoiceProvider
    {
        private readonly string _outputFile = $@"F:\Development\Clients\BridgeportClaims\Envision\SampleFiles\Invoice_{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
        private const int DefaultFontStyle = 0;

        public void ProcessInvoice(InvoicePdfDto data)
        {
            DisposableService.Using(() => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
                {
                    var reader = new PdfReader(resourceStream.ToBytes());
                    var file = _outputFile;
                    reader.SelectPages("1");
                    var stamper = new PdfStamper(reader, new FileStream(file, FileMode.Create));
                    var contentByte = stamper.GetOverContent(1);
                    StampText(data.BillToName, 341, 770, contentByte);
                    StampText(data.BillToAddress1, 341, 759.75f, contentByte);
                    if (data.BillToAddress2.IsNotNullOrWhiteSpace())
                    {
                        StampText(data.BillToAddress2, 341, 749.5f, contentByte);
                        StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 739.25f, contentByte);
                    }
                    else
                    {
                        StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 749.5f, contentByte);
                    }
                    stamper.Close();
                });
        }

        private static void StampText(string text, float x, float y, PdfContentByte contentByte)
        {
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, 9, DefaultFontStyle, BaseColor.BLACK);
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT, new Phrase(text, font), x, y, 0);
        }
    }
}
