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
            DisposableService.Using(
                () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"),
                resourceStream =>
                {
                    var reader = new PdfReader(resourceStream.ToBytes());
                    var file = _outputFile;
                    // Select 1 page from the original document
                    reader.SelectPages("1");
                    // Create PdfStamper object to copy the contents of the reader into the new PDF.

                    var stamper = new PdfStamper(reader, new FileStream(file, FileMode.Create));
                    // PdfContentByte from stamper to add content to the pages over the original content
                    var contentByte = stamper.GetOverContent(1);
                    //add content to the page using ColumnText
                    var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont);
                    var smallerFont = new Font(baseFont, 8, DefaultFontStyle, BaseColor.BLACK);
                    var phrase = new Phrase(new Chunk($"\nDOB: {data.DateFilledDay}"));
                    phrase.Add(new Chunk("bar"));
                    ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT, new Phrase("MANOR PHARMACY", font), 300, 750.50f, 0);
                    ColumnText.ShowTextAligned(contentByte, Element.ALIGN_LEFT, new Phrase("MANOR PHARMACY", smallerFont), 300, 740, 0);
                    // PdfContentByte from stamper to add content to the pages under the original content
                    //add the image under the original content
                    stamper.Close();
                });
        }

        public void ProcessInvoice(InvoicePdfDto data, int i)
        {
            DisposableService.Using(
                () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BridgeportClaims.Pdf.EmbeddedResources.Invoice.pdf"), resourceStream =>
                {
                    if (null != resourceStream)
                    {
                        DisposableService.Using(() => new PdfReader(resourceStream.ToBytes()), reader =>
                        {
                            DisposableService.Using(() => new Document(reader.GetPageSizeWithRotation(1)), doc =>
                            {
                                DisposableService.Using(() => new FileStream(_outputFile, FileMode.Create, FileAccess.ReadWrite), fs =>
                                    {
                                        DisposableService.Using(() => PdfWriter.GetInstance(doc, fs), writer =>
                                        {
                                            if (!doc.IsOpen())
                                            {
                                                doc.Open();
                                            }
                                            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                            doc.NewPage();
                                            var cb = writer.DirectContent;
                                            cb.BeginText();
                                            try
                                            {
                                                cb.SetFontAndSize(baseFont, 12);
                                                const string line = "Hello, Fucker";
                                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, line, 200, 200, 0);
                                            }
                                            finally
                                            {
                                                cb.EndText();
                                            }
                                            var pageSize = doc.PageSize;
                                            var template = cb.CreateTemplate(pageSize.Width, pageSize.Height);
                                            cb.AddTemplate(template, 0, 0);
                                            if (doc.IsOpen())
                                            {
                                                doc.Close();
                                            }
                                            reader.Close();
                                            fs.Close();
                                            writer.Close();
                                            resourceStream.Close();
                                        });
                                    });
                            });
                        });
                    }
                });
        }
    }
}
