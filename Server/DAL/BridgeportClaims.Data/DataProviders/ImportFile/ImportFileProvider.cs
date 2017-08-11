using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
	public class ImportFileProvider : IImportFileProvider
	{
		private readonly IRepository<VwImportFile> _vwImportFileRepository;

		public ImportFileProvider(IRepository<VwImportFile> vwImportFileRepository)
		{
			_vwImportFileRepository = vwImportFileRepository;
		}

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

		public IList<ImportFileDto> GetImportFileDtos()
		{
			var files = _vwImportFileRepository.GetAll()
				.Select(f => new ImportFileDto
				{
					CreatedOn = Convert.ToDateTime(f.CreatedOnLocal),
					FileExtension = f.FileExtension,
					FileName = f.FileName,
					FileSize = f.FileSize,
					FileType = f.FileType,
					ImportFileId = f.ImportFileId,
					Processed = f.Processed
				}).ToList();
			return files;
			/*var files = new List<ImportFileDto>();
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
			});*/
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
						sqlCommand.ExecuteNonQuery();
					});
			});
		}

		private static string GetFileSize(double byteCount)
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