using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
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
            var now = DateTime.UtcNow.ToMountainTime().ToString("M/d/yyyy");
            ExportDataTableToPdf(dt, fullFilePath, $"Billing Statement {now}");
            Process.Start(fullFilePath);
            return fullFilePath;
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
                        var btntHead = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fHead = new Font(btntHead, 16, 1, BaseColor.GRAY);
                        var bParagrapsh = new Paragraph {Alignment = Element.ALIGN_LEFT};
                        bParagrapsh.Add(new Chunk(header.ToUpper(), fHead));
                        doc.Add(bParagrapsh);

                        // Author
                        var aParagraph = new Paragraph();
                        var bAuthor = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fntAuthor = new Font(bAuthor, 8, 2, BaseColor.GRAY);
                        aParagraph.Alignment = Element.ALIGN_LEFT;
                        aParagraph.Add(new Chunk("Author: Dotnet Mob", fntAuthor));
                        aParagraph.Add(new Chunk("\nRun Date : " + DateTime.UtcNow.ToMountainTime().ToShortDateString(),
                            fntAuthor));
                        doc.Add(aParagraph);

                        // Add a line seperation
                        var p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 00.0F, BaseColor.BLACK, Element.ALIGN_LEFT, Element.ALIGN_RIGHT)));
                        doc.Add(p);

                        // Add line break
                        doc.Add(new Chunk("\n", fHead));

                        // Write the table
                        var table = new PdfPTable(dt.Columns.Count);

                        // Table Header
                        var btnColumnHeader = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fntColumnHeader = new Font(btnColumnHeader, 10, 1, BaseColor.WHITE);
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            var cell = new PdfPCell();
                            cell.BackgroundColor = BaseColor.GRAY;
                            cell.AddElement(new Chunk(dt.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                            table.AddCell(cell);
                        }

                        // Table data
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            for (var j = 0; j < dt.Columns.Count; j++)
                            {
                                table.AddCell(dt.Rows[i][j].ToString());

                            }
                        }

                        doc.Add(table);
                        doc.Close();
                        writer.Close();
                        fs.Close();
                    }
                }
            }
        }
    }
}