using System;
using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BridgeportClaims.Excel.Factories
{
    public static class ExcelFactory
    {
        private const string CurrencyFormat = "$###,###,##0.00";

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

        public static string GetBillingStatementExcelFilePathFromDataTable(DataTable dt, string workSheetName, string fileName) =>
            DisposableService.Using(() => new ExcelPackage(), pck =>
            {
                var excelWorksheet = pck.Workbook?.Worksheets?.Add(workSheetName);
                if (null == excelWorksheet)
                    throw new Exception("Something went wrong, could not create an Excel worksheet.");
                // excelWorksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
                excelWorksheet.Cells[1, 1].Value = $"Billing Statement {DateTime.UtcNow.ToMountainTime():M/d/yyyy}";
                excelWorksheet.Cells[1, 2].Value = "Jamie Valone";
                excelWorksheet.Cells[1, 3].Value = "DOB 1/10/1968";
                excelWorksheet.Cells["A5"].LoadFromDataTable(dt, true, TableStyles.Medium4);
                excelWorksheet.Column(1).Style.Numberformat.Format = "MM/dd/yyyy";
                excelWorksheet.Column(2).AutoFit();
                excelWorksheet.Column(3).AutoFit();
                excelWorksheet.Column(4).AutoFit();
                excelWorksheet.Column(5).AutoFit();
                excelWorksheet.Column(6).AutoFit();
                excelWorksheet.Column(7).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(7).AutoFit();
                excelWorksheet.Column(8).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(8).AutoFit();
                var rowCount = excelWorksheet.Dimension.End.Row;
                var colCount = excelWorksheet.Dimension.End.Column;
                excelWorksheet.Cells[rowCount + 1, colCount].Formula = $"=SUM(I6:I{rowCount})";
                excelWorksheet.Column(9).Style.Numberformat.Format = CurrencyFormat;
                excelWorksheet.Column(9).AutoFit();
                var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
                return DisposableService.Using(() => File.Create(fullFilePath), stream =>
                {
                    pck.SaveAs(stream);
                    return fullFilePath;
                });
            });
    }
}