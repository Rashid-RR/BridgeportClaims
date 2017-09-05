using System.IO;
using System.Data;
using ExcelDataReader;
using BridgeportClaims.Common.Disposable;

namespace BridgeportClaims.Excel.Adapters
{
	public static class ExcelDataReaderAdapter
	{
		public static DataTable ReadExcelFileIntoDataTable(byte[] bytes) => DisposableService.Using(
			() => ExcelReaderFactory.CreateReader(new MemoryStream(bytes)), reader =>
			{
				var result = reader.AsDataSet();
				return result.Tables[0];
			});

	    public static DataTable ReadExcelFileIntoDataTable(string fullFilePath)
	    {
	        return DisposableService.Using(() => File.Open(fullFilePath, FileMode.Open, FileAccess.Read), stream =>
	        {
	            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
	            var result = excelReader.AsDataSet();
	            return result.Tables[0];
	        });
	    }
	}
}
