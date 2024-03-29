﻿using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Reflection;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TableStyles = OfficeOpenXml.Table.TableStyles;

namespace BridgeportClaims.Excel.Factories
{
    public static class ExcelFactory
    {
        private const string CurrencyFormat = "$###,###,##0.00";
        private const string DateFormat = "MM/dd/yyyy";

        public static string GetExcelFilePathFromDataTable(DataTable dt, string workSheetName, string fileName) => 
            DisposableService.Using(() => new ExcelPackage(), pck =>
            {
                var excelWorksheet = pck.Workbook?.Worksheets?.Add(workSheetName);
                if (null == excelWorksheet)
                    throw new Exception("Something went wrong, could not create an Excel worksheet.");
                excelWorksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
                var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
                return DisposableService.Using(() => File.Create(fullFilePath), stream =>
                {
                    pck.SaveAs(stream);
                    return fullFilePath;
                });
            });

        public static string GetBillingStatementExcelFilePathFromDataTable(DataTable dt, string workSheetName, string fileName, BillingStatementDto dto) =>
            DisposableService.Using(() => new ExcelPackage(), pck =>
            {
                if (null == dto)
                {
                    throw new ArgumentNullException(nameof(dto));
                }
                dt.TableName = "BillingStatementDataTable";
                var excelWorksheet = pck.Workbook?.Worksheets?.Add(workSheetName);
                if (null == excelWorksheet)
                {
                    throw new Exception("Something went wrong, could not create an Excel worksheet.");
                }
                excelWorksheet.Cells[1, 1].Value = $"Billing Statement {DateTime.UtcNow.ToMountainTime():M/d/yyyy}";
                excelWorksheet.Cells[1, 1].Style.Font.Bold = true;
                excelWorksheet.Cells[2, 1].Value = $"{dto.FirstName} {dto.LastName}";
                excelWorksheet.Cells[2, 1].Style.Font.Bold = true;
                excelWorksheet.Cells[3, 1].Value = dto.DateOfBirth.HasValue ? $"DOB {dto.DateOfBirth.Value:M/d/yyyy}" : string.Empty;
                excelWorksheet.Cells[3, 1].Style.Font.Bold = true;
                excelWorksheet.Cells[4, 1].Value = dto.DateOfInjury.HasValue ? $"DOI {dto.DateOfInjury.Value:M/d/yyyy}" : string.Empty;
                excelWorksheet.Cells[4, 1].Style.Font.Bold = true;
                excelWorksheet.Cells["A7"].LoadFromDataTable(dt, true);
                var rowCount = excelWorksheet.Dimension.End.Row;
                var colCount = excelWorksheet.Dimension.End.Column;
                var tableHeaders = excelWorksheet.Cells[7, 1, 7, colCount];
                tableHeaders.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                var allCells = excelWorksheet.Cells[1, 1, rowCount, colCount];
                allCells.AutoFilter = false;
                allCells.AutoFitColumns();
                allCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                excelWorksheet.Column(1).Style.Numberformat.Format = DateFormat;
                excelWorksheet.Column(2).AutoFit();
                excelWorksheet.Column(3).AutoFit();
                excelWorksheet.Column(4).AutoFit();
                excelWorksheet.Column(5).AutoFit();
                excelWorksheet.Column(colCount - 3).Width = 15;
                excelWorksheet.Column(colCount - 3).Style.Numberformat.Format = DateFormat;
                excelWorksheet.Column(colCount - 2).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(colCount - 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                excelWorksheet.Column(colCount - 2).AutoFit();
                excelWorksheet.Column(colCount - 1).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(colCount - 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                excelWorksheet.Column(colCount - 1).AutoFit();
                excelWorksheet.Column(colCount).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(colCount).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                excelWorksheet.Column(colCount).AutoFit();
                var totalCell = excelWorksheet.Cells[rowCount + 2, colCount - 3];
                totalCell.Value = "Total:";
                totalCell.Style.Font.Bold = true;
                totalCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                var invAmtTotalCell = excelWorksheet.Cells[rowCount + 2, colCount - 2];
                invAmtTotalCell.Formula = $"=SUM(G6:G{rowCount})";
                AddStyleToTotalCell(invAmtTotalCell);
                var amtPaidTotalCell = excelWorksheet.Cells[rowCount + 2, colCount - 1];
                amtPaidTotalCell.Formula = $"=SUM(H6:H{rowCount})";
                AddStyleToTotalCell(amtPaidTotalCell);
                var outstandingTotalCell = excelWorksheet.Cells[rowCount + 2, colCount];
                outstandingTotalCell.Formula = $"=SUM(I6:I{rowCount})";
                AddStyleToTotalCell(outstandingTotalCell);
                var fullFilePath = Path.Combine(Path.GetTempPath(), fileName + ".xlsx");
                Image img = null;
                excelWorksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                excelWorksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                excelWorksheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                excelWorksheet.Cells[4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                DisposableService.Using(
                    () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("BridgeportClaims.Excel.EmbeddedResources.logo.png"),
                    resourceStream => { img = Image.FromStream(resourceStream); });
                if (null != img)
                {
                    var pic = excelWorksheet.Drawings.AddPicture("logo.png", img);
                    pic.SetPosition(0, 10, colCount - 2, 15);
                }
                return DisposableService.Using(() => File.Create(fullFilePath), stream =>
                {
                    pck.SaveAs(stream);
                    return fullFilePath;
                });
            });

        private static void AddStyleToTotalCell(ExcelRangeBase cell)
        {
            cell.Calculate();
            cell.Style.Font.Bold = true;
            cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            cell.Style.Border.Top.Color.SetColor(Color.Black);
            cell.Style.Border.Bottom.Color.SetColor(Color.Black);
        }
    }
}