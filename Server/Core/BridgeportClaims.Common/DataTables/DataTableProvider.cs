using System.Data;

namespace BridgeportClaims.Common.DataTables
{
	public static class DataTableProvider
	{
		public static DataTable FormatDataTableForPaymentImport(DataTable dt, bool firstRowIsHeader)
		{
			if (firstRowIsHeader)
			{
				var row = dt.Rows[0];
				dt.Rows.Remove(row);
			}
			var dtCloned = dt.Clone();
			dtCloned.Columns[0].DataType = typeof(string);
			dtCloned.Columns[1].DataType = typeof(string);
			dtCloned.Columns[2].DataType = typeof(string);
			dtCloned.Columns[3].DataType = typeof(string);
			dtCloned.Columns[4].DataType = typeof(string);
			dtCloned.Columns[5].DataType = typeof(string);
			dtCloned.Columns[6].DataType = typeof(string);
			dtCloned.Columns[7].DataType = typeof(string);
			dtCloned.Columns[8].DataType = typeof(string);
			dtCloned.Columns[9].DataType = typeof(string);
			// Get rid of any columns beyond 10.
			const int desiredSize = 10;
			while (dtCloned.Columns.Count > desiredSize)
			    dtCloned.Columns.RemoveAt(desiredSize);
			foreach (DataRow row in dt.Rows)
				dtCloned.ImportRow(row);
			return dtCloned;
		}
	}
}