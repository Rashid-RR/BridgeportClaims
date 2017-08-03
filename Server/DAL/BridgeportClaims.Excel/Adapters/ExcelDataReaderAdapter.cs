using System.Data;
using System.IO;
using BridgeportClaims.Common.Disposable;
using ExcelDataReader;

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
	}
}
