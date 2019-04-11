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
        private const int DefaultFontStyle = 0;
        private static readonly BaseFont BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        private static readonly Font Font = new Font(BaseFont, 9, DefaultFontStyle, BaseColor.BLACK);

        public bool ProcessInvoice(InvoicePdfDto data, string targetPath)
        {
            var success = true;
            DisposableService.Using(() => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
                {
                    var reader = new PdfReader(resourceStream.ToBytes());
                    reader.SelectPages("1");
                    var stamper = new PdfStamper(reader, new FileStream(targetPath, FileMode.Create));
                    try
                    {
                        var contentByte = stamper.GetOverContent(1);
                        StampText(data.BillToName, 341, 770, contentByte);
                        StampText(data.BillToAddress1, 341, 759.75f, contentByte);
                        if (data.BillToAddress2.IsNotNullOrWhiteSpace())
                        {
                            StampText(data.BillToAddress2, 341, 749.5f, contentByte);
                            StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341,
                                739.25f, contentByte);
                        }
                        else
                        {
                            StampText($"{data.BillToCity}, {data.BillToStateCode} {data.BillToPostalCode}", 341, 749.5f,
                                contentByte);
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                    finally
                    {
                        stamper.Close();
                    }
                });
            return success;
        }

        private static void StampText(string text, float x, float y, PdfContentByte contentByte)
        {
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT, new Phrase(text, Font), x, y, 0);
        }
    }
}
