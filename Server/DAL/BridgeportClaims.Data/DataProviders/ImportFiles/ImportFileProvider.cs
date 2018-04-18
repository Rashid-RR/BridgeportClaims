using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;
using BridgeportClaims.CsvReader.CsvReaders;

namespace BridgeportClaims.Data.DataProviders.ImportFiles
{
	public class ImportFileProvider : IImportFileProvider
	{
	    private readonly IRepository<ImportFile> _importFileRepository;
		private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
		private readonly ICsvReaderProvider _csvReaderProvider;
		private readonly IRepository<ImportFileType> _importFileTypeRepository;

	    public ImportFileProvider(IRepository<ImportFile> importFileRepository,
	        ICsvReaderProvider csvReaderProvider, 
	        IRepository<ImportFileType> importFileTypeRepository)
	    {
	        _importFileRepository = importFileRepository;
	        _csvReaderProvider = csvReaderProvider;
	        _importFileTypeRepository = importFileTypeRepository;
	    }


	    public void LakerImportFileProcedureCall(DataTable dataTable, bool debugOnly = false)
		{
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				DisposableService.Using(() => new SqlCommand("etl.uspStageNewLakerFile", conn), command =>
				{
					var udt = new SqlParameter
					{
						ParameterName = "@Base",
						TypeName = "etl.udtLakerFile",
						Direction = ParameterDirection.Input,
						Value = dataTable,
						SqlDbType = SqlDbType.Structured
					};
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(udt);
					command.CommandTimeout = 600; // 10 minutes.
					command.Parameters.Add(new SqlParameter
					{
						ParameterName = "@DebugOnly",
						SqlDbType = SqlDbType.Bit,
						DbType = DbType.Boolean,
						Value = debugOnly
					});
					if (conn.State != ConnectionState.Open)
						conn.Open();
					command.ExecuteNonQuery();
				});
			});
		}

		public string GetLakerFileTemporaryPath(Tuple<string, byte[]> tuple)
		{
			var methodName = MethodBase.GetCurrentMethod().Name;
			if (cs.AppIsInDebugMode)
				Logger.Value.Info($"Entering the \"{methodName}\" method at: {DateTime.UtcNow.ToMountainTime():M/d/yyyy h:mm:ss tt}");
			var oldestLakeFileName = _importFileRepository.GetMany(x => !x.Processed)
				.Where(f => null != f.FileName && f.FileName.StartsWith(c.LakeFileNameStartsWithString))
				.OrderBy(f => f.CreatedOnUtc)
				.Select(f => f.FileName).FirstOrDefault();
			if (!oldestLakeFileName.IsNotNullOrWhiteSpace()) return string.Empty;
			if (null == tuple || tuple.Item1 != oldestLakeFileName) return string.Empty;
			var fullFilePath = string.Empty;
			if (null != oldestLakeFileName)
				fullFilePath = Path.Combine(Path.GetTempPath(), oldestLakeFileName);
			File.WriteAllBytes(fullFilePath, tuple.Item2);
			if (File.Exists(fullFilePath))
				return fullFilePath;
			throw new IOException($"Error. Unable to save the Laker CSV file to {fullFilePath}");
		}

		public DataTable RetreiveDataTableFromLatestLakerFile(string fullFilePathOfLatestLakerFile)
		{
			if (fullFilePathOfLatestLakerFile.IsNullOrWhiteSpace())
				throw new Exception("The full file path to the latest Laker CSV doesn't exist.");
			var dt = _csvReaderProvider.ReadCsvFile(fullFilePathOfLatestLakerFile);
			if (null == dt) throw new Exception($"Could not read CSV into Data Table from {fullFilePathOfLatestLakerFile}");
			// Cleanup temporary file.
			if (File.Exists(fullFilePathOfLatestLakerFile))
				File.Delete(fullFilePathOfLatestLakerFile);
			return dt;
		}

		public void DeleteImportFile(int importFileId)
		{
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), connection =>
			{
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
					if (connection.State != ConnectionState.Open)
						connection.Open();
					cmd.ExecuteNonQuery();
				});
			});
		}

		/// <summary>
		/// Finds the bytes of a Laker file in the database (if one exists).
		/// Needs to handle it gracefully if none exists.
		/// </summary>
		/// <returns></returns>
		public Tuple<string, byte[]> GetOldestLakerFileBytes()
		{
			return DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("dbo.uspGetOldestLakerFileBytes", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 90; // 90 seconds, instead of 30
					if (conn.State != ConnectionState.Open)
						conn.Open();
					return DisposableService.Using(cmd.ExecuteReader, sqlDataReader =>
					{
						byte[] bytes = null;
						string fileName = null;
						var fileBytesOrdinal = sqlDataReader.GetOrdinal("FileBytes");
						var fileNameOrdinal = sqlDataReader.GetOrdinal("FileName");
						while (sqlDataReader.Read())
						{
							if (!sqlDataReader.IsDBNull(fileBytesOrdinal))
								bytes = (byte[]) sqlDataReader["FileBytes"];
							if (!sqlDataReader.IsDBNull(fileNameOrdinal))
								fileName = sqlDataReader.GetString(fileNameOrdinal);
							return new Tuple<string, byte[]>(fileName, bytes);
						}
						return null;
					});
				});
			});
		}

		public IList<ImportFileDto> GetImportFileDtos()
			=> DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), connection =>
			{
				var files = new List<ImportFileDto>();
				return DisposableService.Using(() => new SqlCommand("dbo.uspGetImportFile", connection), sqlCommand =>
				{
					sqlCommand.CommandType = CommandType.StoredProcedure;
					if (connection.State != ConnectionState.Open)
						connection.Open();
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

		public void MarkFileProcessed(string fileName)
		{
			const string sql = @"UPDATE i SET i.Processed = 1 FROM util.ImportFile AS i WHERE i.[FileName] = @FileName;";
			DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), connection =>
			{
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
					if (connection.State != ConnectionState.Open)
						connection.Open();
					cmd.ExecuteNonQuery();
				});
			});
		}

		public void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, 
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
				DisposableService.Using(() => new SqlCommand(
					@"INSERT [util].[ImportFile] ([FileBytes],[FileName],[FileExtension],
							[FileSize],[ImportFileTypeID],[Processed],[CreatedOnUTC],[UpdatedOnUTC])
					VALUES (@FileBytes, @FileName,@FileExtension,@FileSize,@ImportFileTypeID,
							@Processed,SYSUTCDATETIME(),SYSUTCDATETIME())",
					connection),
					sqlCommand =>
					{
						var tuple = GetImportFileTypeIdAndProcessedBoolByFileName(fileName);
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
						sqlCommand.Parameters.Add("@ImportFileTypeID", SqlDbType.Int, 1).Value = tuple.Item1;
						sqlCommand.Parameters.Add("@Processed", SqlDbType.Bit).Value = tuple.Item2;
						if (connection.State != ConnectionState.Open)
							connection.Open();
						sqlCommand.ExecuteNonQuery();
					});
			});
		}

		private Tuple<int, bool> GetImportFileTypeIdAndProcessedBoolByFileName(string fileName)
		{
			string code;
			var processed = false;
			// Laker Import	    LI
			// Payment Import   PI
			// Other            OT
			if (fileName.StartsWith("Billing_Claim_File_"))
				code = c.LakerImportImportFileTypeCode;
			else if (fileName.EndsWith("Payments.xlsx"))
				code = c.PaymentImportFileTypeCode;
			else
			{
				code = c.OtherImportFileTypeCode;
				processed = true;
			}
			var result = _importFileTypeRepository.GetSingleOrDefault(x => x.Code == code);
			if (null == result)
				throw new Exception("Error. The util.ImportFileType table does not have any records WHERE Code == 'OT'.");
			return new Tuple<int, bool>(result.ImportFileTypeId, processed);
		}

		public void EtlLakerFile(string fileName)
		{
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), connection =>
			{
				DisposableService.Using(() => new SqlCommand("etl.uspProcessLakerFile", connection), cmd =>
				{
					var fileNameParam = cmd.CreateParameter();
					fileNameParam.Direction= ParameterDirection.Input;
					fileNameParam.Value = fileName ?? (object) DBNull.Value;
					fileNameParam.ParameterName = "@FileName";
					fileNameParam.DbType = DbType.AnsiString;
					fileNameParam.SqlDbType = SqlDbType.NVarChar;
					fileNameParam.Size = 255;
					cmd.Parameters.Add(fileNameParam);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 1800; // 30 Minutes.
					if (connection.State != ConnectionState.Open)
						connection.Open();
					cmd.ExecuteNonQuery();
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