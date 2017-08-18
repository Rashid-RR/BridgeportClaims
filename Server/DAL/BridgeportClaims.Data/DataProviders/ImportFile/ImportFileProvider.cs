using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
	public class ImportFileProvider : IImportFileProvider
	{
		private readonly IMemoryCacher _memoryCacher;

		public ImportFileProvider(IMemoryCacher memoryCacher)
		{
			_memoryCacher = memoryCacher;
		}

		public void DeleteImportFile(int importFileId)
		{
			// Remove cached entries
			_memoryCacher.DeleteIfExists(c.ImportFileDatabaseCachingKey);
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

		public IList<ImportFileDto> GetImportFileDtos()
		{
			// Get Items from Cache if they exist there.
			var cachedFiles = _memoryCacher.AddOrGetExisting(c.ImportFileDatabaseCachingKey, () =>
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
							var importFileIdOrdinal = reader.GetOrdinal("ImportFileID");
							var fileNameOrdinal = reader.GetOrdinal("FileName");
							var fileExtensionOrdinal = reader.GetOrdinal("FileExtension");
							var fileSizeOrdinal = reader.GetOrdinal("FileSize");
							var fileTypeOrdinal = reader.GetOrdinal("FileType");
							var processedOrdinal = reader.GetOrdinal("Processed");
							var createdOnLocalOrdinal = reader.GetOrdinal("CreatedOnLocal");

							while (reader.Read())
							{
								var file = new ImportFileDto
								{
									ImportFileId = reader.GetInt32(importFileIdOrdinal),
									FileName = reader.GetString(fileNameOrdinal),
									FileSize = reader.GetString(fileSizeOrdinal),
									FileType = reader.GetString(fileTypeOrdinal),
									Processed = reader.GetBoolean(processedOrdinal),
									CreatedOn = !reader.IsDBNull(createdOnLocalOrdinal)
										? reader.GetDateTime(createdOnLocalOrdinal)
										: DateTime.Now
								};
								if (!reader.IsDBNull(fileExtensionOrdinal))
									file.FileExtension = reader.GetString(fileExtensionOrdinal);
								files.Add(file);
							}
							var retList = files.OrderByDescending(x => x.CreatedOn).ToList();
							return retList;
						});
					});
				});
			});
			return cachedFiles;
		}

		public void MarkFileProcessed(string fileName)
		{
			// Remove cached entries
			_memoryCacher.DeleteIfExists(c.ImportFileDatabaseCachingKey);
			const string sql = @"UPDATE i SET i.Processed = 1 FROM util.ImportFile AS i WHERE i.[FileName] = @FileName;";
			DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), connection =>
			{
				connection.Open();
				DisposableService.Using(() => new SqlCommand(sql, connection), cmd =>
				{
					cmd.CommandType = CommandType.Text;
					var fileNameParam = new SqlParameter
					{
						Value = fileName,
						SqlDbType = SqlDbType.VarChar,
						DbType = DbType.String,
						ParameterName = "@FileName"
					};
					cmd.Parameters.Add(fileNameParam);
					cmd.ExecuteNonQuery();
				});
			});
		}

		public void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, 
			string fileDescription)
		{
			// Remove cached entries
			_memoryCacher.DeleteIfExists(c.ImportFileDatabaseCachingKey);
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
				DisposableService.Using(() => new SqlCommand(
					@"INSERT [util].[ImportFile] ([FileBytes],[FileName],[FileExtension],
							[FileSize],[ImportFileTypeID],[Processed],[CreatedOnUTC],[UpdatedOnUTC])
					VALUES (@FileBytes, @FileName,@FileExtension,@FileSize,@ImportFileTypeID,
							@Processed,SYSUTCDATETIME(),SYSUTCDATETIME())",
					connection),
					sqlCommand =>
					{
						sqlCommand.CommandType = CommandType.Text;
						sqlCommand.Parameters.Add("@FileBytes", SqlDbType.VarBinary, file.Length).Value = file;
						sqlCommand.Parameters.Add("@FileName", SqlDbType.NVarChar, fileName.Length).Value = fileName;
						if (fileExtension.IsNullOrWhiteSpace())
							throw new Exception("The \"fileExtension\" parameter cannot be null");
						sqlCommand.Parameters.Add("@FileExtension", SqlDbType.VarChar, 
							fileExtension.Length).Value = fileExtension;
						var fileSize = GetFileSize(file.Length);
						sqlCommand.Parameters.Add("@FileSize", SqlDbType.VarChar,
							fileSize.Length).Value = fileSize;
						sqlCommand.Parameters.Add("@ImportFileTypeID", SqlDbType.Int, 1).Value =
							fileExtension == ".csv" ? 1 : 2; // TODO: Make this dynamic.
						sqlCommand.Parameters.Add("@Processed", SqlDbType.Bit).Value = false;
						sqlCommand.ExecuteNonQuery();
					});
			});
		}

		public static string GetFileSize(double byteCount)
		{
			var size = "0 Bytes";
			if (byteCount >= 1073741824.0)
				size = $"{byteCount / 1073741824.0:##.##}" + " GB";
			else if (byteCount >= 1048576.0)
				size = $"{byteCount / 1048576.0:##.##}" + " MB";
			else if (byteCount >= 1024.0)
				size = $"{byteCount / 1024.0:##.##}" + " KB";
			else if (byteCount > 0 && byteCount < 1024.0)
				size = byteCount.ToString(CultureInfo.InvariantCulture) + " Bytes";
			return size;
		}
	}
}