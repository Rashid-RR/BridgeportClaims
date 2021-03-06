﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using NLog;

namespace BridgeportClaims.Pdf.Factories
{
    public class PdfFactory : IPdfFactory
    {
        private const int DefaultFontSize = 12;
        private const int DefaultFontStyle = 0;
        private const int DefaultCellFontSize = 6;
        private const int DefaultCellFontStyle = 1; // Bold
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public string GeneratePdf(DataTable dt)
        {
            var fileName = $"{Guid.NewGuid()}.pdf";
            var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
            var now = DateTime.UtcNow.ToMountainTime().ToString("M/d/yyyy");
            ExportDataTableToPdf(dt, fullFilePath, $"Billing Statement {now}", "Jordan Gurney", DateTime.Now); // TODO: Fix
            Process.Start(fullFilePath);
            return fullFilePath;
        }

        public bool MergePdf(IEnumerable<Uri> fileUrls, string targetPdf)
        {
            var merged = true;
            DisposableService.Using(() => new FileStream(targetPdf, FileMode.Create), stream =>
            {
                DisposableService.Using(() => new Document(), doc =>
                {
                    DisposableService.Using(() => new PdfCopy(doc, stream), pdf =>
                    {
                        PdfReader reader = null;
                        try
                        {
                            if (!doc.IsOpen())
                            {
                                doc.Open();
                            }
                            fileUrls.ForEach(url =>
                            {
                                reader = new PdfReader(url);
                                pdf.AddDocument(reader);
                                reader.Close();
                            });
                        }
                        catch (Exception ex)
                        {
                            merged = false;
                            reader?.Close();
                            Logger.Value.Error(ex);
                        }
                        finally
                        {
                            doc.Close();
                        }
                    });
                });
            });
            return merged;
        }

        private static void ExportDataTableToPdf(DataTable dt, string pdfPath, string header, string claimantName, DateTime dateOfBirth)
        {
            DisposableService.Using(() => new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None), fs =>
            {
                                                                                    // Margins
                DisposableService.Using(() => new Document(PageSize.LETTER.Rotate(), 0, 0, 10, 10), doc =>
                {
                    DisposableService.Using(() => PdfWriter.GetInstance(doc, fs), writer =>
                    {
                        if (!doc.IsOpen())
                            doc.Open();
                        doc.NewPage();

                        // Report Header.
                        var ebtnHead = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fHead = new Font(ebtnHead, DefaultFontSize, DefaultFontStyle, BaseColor.BLACK);
                        var bParagraphs = new Paragraph {Alignment = Element.ALIGN_LEFT, IndentationLeft = 80, PaddingTop = -100};
                        bParagraphs.Add(new Chunk(header.ToUpper(), fHead));
                        doc.Add(bParagraphs);


                        // Author
                        var aParagraph = new Paragraph {IndentationLeft = 80};
                        var bAuthor = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fntAuthor = new Font(bAuthor, DefaultFontSize, DefaultFontStyle, BaseColor.BLACK);
                        aParagraph.Alignment = Element.ALIGN_LEFT;
                        aParagraph.PaddingTop = -200;
                        aParagraph.Add(new Chunk(claimantName, fntAuthor));
                        aParagraph.Add(new Chunk($"\nDOB: {dateOfBirth:M/d/yyyy}", fntAuthor));
                        doc.Add(aParagraph);

                        // Add a line separation
                        var p = new Paragraph(new Chunk(new LineSeparator(0.0F, 00.0F, BaseColor.BLACK, Element.ALIGN_CENTER, Element.ALIGN_CENTER))); 
                        doc.Add(p);

                        // Add line break
                        doc.Add(new Chunk("\n", fHead));

                        // Write the table
                        var table = new PdfPTable(dt.Columns.Count) {HeaderRows = 1};
                        table.DefaultCell.Border = Rectangle.NO_BORDER;

                        // Table Header
                        var btnColumnHeader =
                            BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var fntColumnHeader = new Font(btnColumnHeader, DefaultCellFontSize, DefaultCellFontStyle, BaseColor.BLACK);
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            var cell = new PdfPCell {BackgroundColor = BaseColor.YELLOW };
                            cell.AddElement(new Chunk(dt.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                            table.AddCell(cell);
                        }

                        // Table data
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            for (var j = 0; j < dt.Columns.Count; j++)
                            {
                                if (dt.Rows[i][j].ToString().IsNullOrWhiteSpace())
                                {
                                    continue;
                                }
                                var row = dt.Rows[i][j].ToString();
                                table.AddCell(row);
                            }
                        }
                        doc.Add(table);
                        doc.Close();
                        writer.Close();
                        fs.Close();
                    });
                });
            });      
        }
    }
}