using System.Data;
using System.Data.SqlClient;
using System.IO;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
    public static class ImportFileProvider
    {
        public static void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, string fileDescription)
        {
            byte[] file;
            using (var reader = new BinaryReader(stream))
            {
                file = reader.ReadBytes((int)stream.Length);
            }
            using (var connection = new SqlConnection(ConfigService.GetDbConnStr()))
            {
                connection.Open();
                using (var sqlCommand = new SqlCommand("INSERT [dbo].[ImportFile] ([FileBytes], " +
                    "[FileName], [FileExtension], [FileDescription]) Values (@File, @FileName, @FileExtension, @FileDescription);",
                    connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
                    sqlCommand.Parameters.Add("@FileName", SqlDbType.NVarChar, fileName.Length).Value = fileName;
                    if (fileExtension.IsNotNullOrWhiteSpace())
                    {
                        sqlCommand.Parameters.Add("@FileExtension", SqlDbType.VarChar,
                            fileExtension.Length).Value = fileExtension;
                    }
                    else
                    {
                        sqlCommand.CommandText = sqlCommand.CommandText.Replace("@FileExtension", "NULL");
                    }
                    if (fileDescription.IsNotNullOrWhiteSpace())
                    {
                        sqlCommand.Parameters.Add("@FileDescription", SqlDbType.VarChar,
                            fileDescription.Length).Value = fileDescription;
                    }
                    else
                    {
                        sqlCommand.CommandText = sqlCommand.CommandText.Replace("@FileDescription", "NULL");
                    }
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}