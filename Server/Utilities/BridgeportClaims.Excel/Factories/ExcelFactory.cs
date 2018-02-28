using System;
using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BridgeportClaims.Excel.Factories
{
    public static class ExcelFactory
    {
        public static string GetExcelFilePathFromDataTable(DataTable dt, string workSheetName, string fileName) => 
            DisposableService.Using(() => new ExcelPackage(), pck =>
            {
                var wsDt = pck.Workbook?.Worksheets?.Add(workSheetName);
                if (null == wsDt)
                    throw new Exception("Something went wrong, could not create an Excel worksheet.");
                wsDt.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
                var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
                return DisposableService.Using(() => File.Create(fullFilePath), stream =>
                {
                    pck.SaveAs(stream);
                    return fullFilePath;
                });
            });
    }
}