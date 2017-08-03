using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
	public static class ImportFileProvider
	{
		public static void DeleteImportFile(int importFileId)
		{
			DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), connection =>
			{
				connection.Open();
				DisposableService.Using(() => new SqlCommand("uspDeleteImportFile", connection), cmd => 
				{
					cmd.CommandType = CommandType.StoredProcedure;
					var importFileIdParam = new SqlParameter
					{
						Value = importFileId,
						SqlDbType = SqlDbType.Int,
						DbType = DbType.Int32,
						ParameterName = "@ImportFileID"
					};
					cmd.Parameters.Add(importFileIdParam);
					cmd.ExecuteNonQuery();
				});
			});
		}

		public static IList<ImportFileDto> GetImportFileDtos()
		{
			var files = new List<ImportFileDto>();
			return DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), connection =>
			{
				connection.Open();
				return DisposableService.Using(() => new SqlCommand("uspGetImportFile", connection), sqlCommand =>
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					return DisposableService.Using(sqlCommand.ExecuteReader, reader =>
					{
						var fileImportIdOrdinal = reader.GetOrdinal("ImportFileID");
						var fileNameOrdinal = reader.GetOrdinal("FileName");
						var fileExtensionOrdinal = reader.GetOrdinal("FileExtension");
						var createdOnOrdinal = reader.GetOrdinal("CreatedOn");
						while (reader.Read())
						{
							var file = new ImportFileDto
							{
								ImportFileId = reader.GetInt32(fileImportIdOrdinal),
								FileName = reader.GetString(fileNameOrdinal),
								CreatedOn = reader.GetDateTime(createdOnOrdinal)
							};
							if (!reader.IsDBNull(fileExtensionOrdinal))
								file.FileExtension = reader.GetString(fileExtensionOrdinal);
							files.Add(file);
						}
						return files.OrderByDescending(x => x.CreatedOn).ToList();
					});
				});
			});
		}

		public static void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, 
			string fileDescription)
		{
			byte[] file = null;
			DisposableService.Using(() => new BinaryReader(stream), reader =>
			{
				file = reader.ReadBytes((int) stream.Length);
			});
			if (null == file)
				throw new ArgumentNullException(nameof(file));
			DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), connection =>
			{
				connection.Open();
				DisposableService.Using(() => new SqlCommand("INSERT [util].[ImportFile] ([FileBytes], " +
				                                             "[FileName], [FileExtension], [FileDescription])" +
				                                             " VALUES (@File, @FileName, @FileExtension, " +
				                                             "@FileDescription);", connection),
					sqlCommand =>
					{
						sqlCommand.CommandType = CommandType.Text;
						sqlCommand.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
						sqlCommand.Parameters.Add("@FileName", SqlDbType.NVarChar, fileName.Length).Value = fileName;
						if (fileExtension.IsNotNullOrWhiteSpace())
							sqlCommand.Parameters.Add("@FileExtension", SqlDbType.VarChar,
								fileExtension.Length).Value = fileExtension;
						else
							sqlCommand.CommandText = sqlCommand.CommandText.Replace("@FileExtension", "NULL");
						if (fileDescription.IsNotNullOrWhiteSpace())
							sqlCommand.Parameters.Add("@FileDescription", SqlDbType.VarChar,
								fileDescription.Length).Value = fileDescription;
						else
							sqlCommand.CommandText = sqlCommand.CommandText.Replace("@FileDescription", "NULL");
						sqlCommand.ExecuteNonQuery();
					});
			});
		}
	}
}