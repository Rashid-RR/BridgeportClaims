using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BridgeportClaims.Excel.ExcelPackageFactory
{
    public class ExcelFactory
    {
        public string GetExcelFilePathFromDataTable(DataTable dt, string workSheetName, string fileName)
        {
            var pck = new ExcelPackage();
            var wsDt = pck.Workbook?.Worksheets?.Add(workSheetName);
            if (null == wsDt)
                throw new Exception("Something went wrong, could not create an Excel worksheet.");
            wsDt.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
            var fullFilePath = Path.Combine(Path.GetTempPath(), fileName);
            using (var stream = File.Create(fullFilePath))
            {
                pck.SaveAs(stream);
                return fullFilePath;
            }
        }
    }
}