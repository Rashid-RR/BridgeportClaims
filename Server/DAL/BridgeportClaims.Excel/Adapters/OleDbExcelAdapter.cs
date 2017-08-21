using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using BridgeportClaims.Common.Extensions;
using p = System.IO.Path;
using f = System.IO.File;

namespace BridgeportClaims.Excel.Adapters
{
    public class OleDbExcelAdapter
    {
        public static DataTable GetDataTableFromExcel(byte[] fileBytes, bool hasHeaders)
        {
            if (null == fileBytes)
                throw new ArgumentNullException(nameof(fileBytes));
            var filePath = p.GetTempFileName();
            if (filePath.IsNullOrWhiteSpace())
                throw new Exception("Error. Cannot obtain a Windows OS temporary file path / name.");
            // Write out the temporary file
            f.WriteAllBytes(filePath, fileBytes);

            var dtexcel = new DataTable();
            var hdr = hasHeaders ? "Yes" : "No";
            var strConn = filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx"
                ? $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0;HDR={hdr};IMEX=0\""
                : $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR={hdr};IMEX=0\"";
            var conn = new OleDbConnection(strConn);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            var schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            var schemaRow = schemaTable?.Rows[0];
            if (null != schemaRow)
            {
                var sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    var query = "SELECT  * FROM [" + sheet + "]";
                    var daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
            }
            conn.Close();
            //Cleanup
            if (f.Exists(filePath))
                f.Delete(filePath);
            return dtexcel;
        }

    }
}