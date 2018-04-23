using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using LakerFileImporter.DAL.ImportFileProvider.Dtos;
using LakerFileImporter.Disposable;
using LakerFileImporter.Logging;
using LakerFileImporter.Security;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;
using NLog;

namespace LakerFileImporter.DAL.ImportFileProvider
{
    internal class ImportFileProvider
    {
        private static readonly Logger Logger = LoggingService.Instance.Logger;

        internal IList<ImportFileDto> GetImportFileDtos()
        {
            var files = GetImportFileDtosInternal();
            if (!cs.AppIsInDebugMode) return files;
            var methodName = MethodBase.GetCurrentMethod().Name;
            var now = DateTime.Now.ToString("G");
            var fileCount = files?.Count;
            Logger.Info(null == fileCount
                ? $"The database query for uploaded Laker Files did not return any results in method {methodName} on {now}."
                : $"The Laker file database query found a total of {fileCount} files in method {methodName} on {now}.");
            return files;
        }

        private static IList<ImportFileDto> GetImportFileDtosInternal()
        {
            try
            {
                var files = new List<ImportFileDto>();
                var connStr = new CompiledSecurityProvider().RawConnectionString;
                return DisposableService.Using(() => new SqlConnection(connStr), connection =>
                {
                    return DisposableService.Using(() => new SqlCommand("dbo.uspGetImportFile", connection),
                        sqlCommand =>
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.CommandTimeout = 600;
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
                                    if (file.FileType == c.LakerFileTypeName)
                                        files.Add(file);
                                }
                                var retList = files.ToList();
                                return retList;
                            });
                        });
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}