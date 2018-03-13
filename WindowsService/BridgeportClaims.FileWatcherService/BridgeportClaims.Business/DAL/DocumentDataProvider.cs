using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Business.Disposable;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;
using cm = BridgeportClaims.Business.ConfigService.ConfigService;

namespace BridgeportClaims.Business.DAL
{
    public class DocumentDataProvider
    {
        private readonly string _dbConnStr = cm.GetDbConnStr();

        internal DocumentDto GetDocumentByFileName(string fullFileName, byte fileTypeId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentSelectByFileName]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var fullFileNameParam = cmd.CreateParameter();
                    fullFileNameParam.Direction = ParameterDirection.Input;
                    fullFileNameParam.Value = fullFileName ?? (object) DBNull.Value;
                    fullFileNameParam.DbType = DbType.String;
                    fullFileNameParam.Size = 4000;
                    fullFileNameParam.SqlDbType = SqlDbType.NVarChar;
                    fullFileNameParam.ParameterName = "@FullFileName";
                    cmd.Parameters.Add(fullFileNameParam);
                    var fileTypeIdParam = cmd.CreateParameter();
                    fileTypeIdParam.Direction = ParameterDirection.Input;
                    fileTypeIdParam.Value = fileTypeId;
                    fileTypeIdParam.DbType = DbType.Byte;
                    fileTypeIdParam.SqlDbType = SqlDbType.TinyInt;
                    fileTypeIdParam.ParameterName = "@FileTypeID";
                    cmd.Parameters.Add(fileTypeIdParam);
                    
                    var retVal = new List<DocumentDto>();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var fileNameOrdinal = reader.GetOrdinal("FileName");
                        var extensionOrdinal = reader.GetOrdinal("Extension");
                        var fileSizeOrdinal = reader.GetOrdinal("FileSize");
                        var creationTimeLocalOrdinal = reader.GetOrdinal("CreationTimeLocal");
                        var lastAccessTimeLocalOrdinal = reader.GetOrdinal("LastAccessTimeLocal");
                        var lastWriteTimeLocalOrdinal = reader.GetOrdinal("LastWriteTimeLocal");
                        var directoryNameOrdinal = reader.GetOrdinal("DirectoryName");
                        var fullFilePathOrdinal = reader.GetOrdinal("FullFilePath");
                        var fileUrlOrdinal = reader.GetOrdinal("FileUrl");
                        var byteCountOrdinal = reader.GetOrdinal("ByteCount");
                        var fileTypeIdOrdinal = reader.GetOrdinal("FileTypeID");
                        while (reader.Read())
                        {
                            var result = new DocumentDto
                            {
                                FileName = !reader.IsDBNull(fileNameOrdinal) ? reader.GetString(fileNameOrdinal) : string.Empty,
                                Extension = !reader.IsDBNull(extensionOrdinal) ? reader.GetString(extensionOrdinal) : string.Empty,
                                FileSize = !reader.IsDBNull(fileSizeOrdinal) ? reader.GetString(fileSizeOrdinal) : string.Empty,
                                CreationTimeLocal = !reader.IsDBNull(creationTimeLocalOrdinal) ? reader.GetDateTime(creationTimeLocalOrdinal) : new DateTime(1900, 1, 1),
                                LastAccessTimeLocal = !reader.IsDBNull(lastAccessTimeLocalOrdinal) ? reader.GetDateTime(lastAccessTimeLocalOrdinal) : new DateTime(1900, 1, 1),
                                LastWriteTimeLocal = !reader.IsDBNull(lastWriteTimeLocalOrdinal) ? reader.GetDateTime(lastWriteTimeLocalOrdinal) : new DateTime(1900, 1, 1),
                                DirectoryName = !reader.IsDBNull(directoryNameOrdinal) ? reader.GetString(directoryNameOrdinal) : string.Empty,
                                FullFilePath = !reader.IsDBNull(fullFilePathOrdinal) ? reader.GetString(fullFilePathOrdinal) : string.Empty,
                                FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : string.Empty,
                                ByteCount = !reader.IsDBNull(byteCountOrdinal) ? reader.GetInt64(byteCountOrdinal) : default(long),
                                FileTypeId = !reader.IsDBNull(fileTypeIdOrdinal) ? reader.GetByte(fileTypeIdOrdinal) : default(byte)
                            };
                            retVal.Add(result);
                        }
                    });
                    if (retVal.Count > 1)
                        throw new Exception($"Error, more than one row was retrieved from the database for file name {fullFileName}, which should be unique.");
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal.SingleOrDefault();
                });
            });

        internal void DeleteDocument(int documentId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentDelete]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    documentIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(documentIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });

        internal int GetDocumentIdByDocumentName(string fullFileName, byte fileTypeId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetDocumentIDByDocumentName]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.DbType = DbType.String;
                    fileNameParam.SqlDbType = SqlDbType.NVarChar;
                    fileNameParam.ParameterName = "@FullFileName";
                    fileNameParam.Size = 4000;
                    fileNameParam.Value = fullFileName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fileNameParam);
                    var fileTypeIdParam = cmd.CreateParameter();
                    fileTypeIdParam.Direction = ParameterDirection.Input;
                    fileTypeIdParam.DbType = DbType.Byte;
                    fileTypeIdParam.SqlDbType = SqlDbType.TinyInt;
                    fileTypeIdParam.Value = fileTypeId;
                    fileTypeIdParam.ParameterName = "@FileTypeID";
                    cmd.Parameters.Add(fileTypeIdParam);
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.Direction = ParameterDirection.Output;
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(documentIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return documentIdParam.Value as int? ?? default(int);
                });
            });

        internal void UpdateDocument(int documentId, string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
            DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl, long byteCount, byte fileTypeId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentUpdate]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    documentIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(documentIdParam);
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.DbType = DbType.AnsiString;
                    fileNameParam.SqlDbType = SqlDbType.VarChar;
                    fileNameParam.ParameterName = "@FileName";
                    fileNameParam.Size = 1000;
                    fileNameParam.Value = fileName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fileNameParam);
                    var extensionParam = cmd.CreateParameter();
                    extensionParam.Direction = ParameterDirection.Input;
                    extensionParam.DbType = DbType.AnsiString;
                    extensionParam.SqlDbType = SqlDbType.VarChar;
                    extensionParam.ParameterName = "@Extension";
                    extensionParam.Value = extension ?? (object)DBNull.Value;
                    extensionParam.Size = 50;
                    cmd.Parameters.Add(extensionParam);
                    var fileSizeParam = cmd.CreateParameter();
                    fileSizeParam.Direction = ParameterDirection.Input;
                    fileSizeParam.DbType = DbType.AnsiString;
                    fileSizeParam.SqlDbType = SqlDbType.VarChar;
                    fileSizeParam.Size = 50;
                    fileSizeParam.ParameterName = "@FileSize";
                    fileSizeParam.Value = fileSize ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fileSizeParam);
                    var creationTimeParam = cmd.CreateParameter();
                    creationTimeParam.DbType = DbType.DateTime2;
                    creationTimeParam.SqlDbType = SqlDbType.DateTime2;
                    creationTimeParam.Direction = ParameterDirection.Input;
                    creationTimeParam.ParameterName = "@CreationTimeLocal";
                    creationTimeParam.Value = creationTime;
                    cmd.Parameters.Add(creationTimeParam);
                    var lastAccessTimeParam = cmd.CreateParameter();
                    lastAccessTimeParam.DbType = DbType.DateTime2;
                    lastAccessTimeParam.SqlDbType = SqlDbType.DateTime2;
                    lastAccessTimeParam.Direction = ParameterDirection.Input;
                    lastAccessTimeParam.ParameterName = "@LastAccessTimeLocal";
                    lastAccessTimeParam.Value = lastAccessTime;
                    cmd.Parameters.Add(lastAccessTimeParam);
                    var lastWriteTimeParam = cmd.CreateParameter();
                    lastWriteTimeParam.Direction = ParameterDirection.Input;
                    lastWriteTimeParam.DbType = DbType.DateTime2;
                    lastWriteTimeParam.SqlDbType = SqlDbType.DateTime2;
                    lastWriteTimeParam.ParameterName = "@LastWriteTimeLocal";
                    lastWriteTimeParam.Value = lastWriteTime;
                    cmd.Parameters.Add(lastWriteTimeParam);
                    var directoryNameParam = cmd.CreateParameter();
                    directoryNameParam.Size = 255;
                    directoryNameParam.DbType = DbType.AnsiString;
                    directoryNameParam.SqlDbType = SqlDbType.VarChar;
                    directoryNameParam.ParameterName = "@DirectoryName";
                    directoryNameParam.Value = directoryName ?? (object)DBNull.Value;
                    directoryNameParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(directoryNameParam);
                    var fullFilePathParam = cmd.CreateParameter();
                    fullFilePathParam.Direction = ParameterDirection.Input;
                    fullFilePathParam.DbType = DbType.String;
                    fullFilePathParam.SqlDbType = SqlDbType.NVarChar;
                    fullFilePathParam.ParameterName = "@FullFilePath";
                    fullFilePathParam.Size = 4000;
                    fullFilePathParam.Value = fullFilePath ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fullFilePathParam);
                    var fileUrlParam = cmd.CreateParameter();
                    fileUrlParam.ParameterName = "@FileUrl";
                    fileUrlParam.Direction = ParameterDirection.Input;
                    fileUrlParam.Value = fileUrl ?? (object)DBNull.Value;
                    fileUrlParam.DbType = DbType.String;
                    fileUrlParam.SqlDbType = SqlDbType.NVarChar;
                    fileUrlParam.Size = 4000;
                    cmd.Parameters.Add(fileUrlParam);
                    var byteCountParam = cmd.CreateParameter();
                    byteCountParam.Direction = ParameterDirection.Input;
                    byteCountParam.Value = byteCount;
                    byteCountParam.DbType = DbType.Int64;
                    byteCountParam.SqlDbType = SqlDbType.BigInt;
                    byteCountParam.ParameterName = "@ByteCount";
                    cmd.Parameters.Add(byteCountParam);
                    var fileTypeIdParam = cmd.CreateParameter();
                    fileTypeIdParam.Direction = ParameterDirection.Input;
                    fileTypeIdParam.Value = fileTypeId;
                    fileTypeIdParam.DbType = DbType.Byte;
                    fileTypeIdParam.SqlDbType = SqlDbType.TinyInt;
                    fileTypeIdParam.ParameterName = "@FileTypeID";
                    cmd.Parameters.Add(fileTypeIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });

        internal int InsertDocument(string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
                DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl, long byteCount, byte fileTypeId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentInsert]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.DbType = DbType.AnsiString;
                    fileNameParam.SqlDbType = SqlDbType.VarChar;
                    fileNameParam.ParameterName = "@FileName";
                    fileNameParam.Size = 1000;
                    fileNameParam.Value = fileName ?? (object) DBNull.Value;
                    cmd.Parameters.Add(fileNameParam);
                    var extensionParam = cmd.CreateParameter();
                    extensionParam.Direction = ParameterDirection.Input;
                    extensionParam.DbType = DbType.AnsiString;
                    extensionParam.SqlDbType = SqlDbType.VarChar;
                    extensionParam.ParameterName = "@Extension";
                    extensionParam.Value = extension ?? (object) DBNull.Value;
                    extensionParam.Size = 50;
                    cmd.Parameters.Add(extensionParam);
                    var fileSizeParam = cmd.CreateParameter();
                    fileSizeParam.Direction = ParameterDirection.Input;
                    fileSizeParam.DbType = DbType.AnsiString;
                    fileSizeParam.SqlDbType = SqlDbType.VarChar;
                    fileSizeParam.Size = 50;
                    fileSizeParam.ParameterName = "@FileSize";
                    fileSizeParam.Value = fileSize ?? (object) DBNull.Value;
                    cmd.Parameters.Add(fileSizeParam);
                    var creationTimeParam = cmd.CreateParameter();
                    creationTimeParam.DbType = DbType.DateTime2;
                    creationTimeParam.SqlDbType = SqlDbType.DateTime2;
                    creationTimeParam.Direction = ParameterDirection.Input;
                    creationTimeParam.ParameterName = "@CreationTimeLocal";
                    creationTimeParam.Value = creationTime;
                    cmd.Parameters.Add(creationTimeParam);
                    var lastAccessTimeParam = cmd.CreateParameter();
                    lastAccessTimeParam.DbType = DbType.DateTime2;
                    lastAccessTimeParam.SqlDbType = SqlDbType.DateTime2;
                    lastAccessTimeParam.Direction = ParameterDirection.Input;
                    lastAccessTimeParam.ParameterName = "@LastAccessTimeLocal";
                    lastAccessTimeParam.Value = lastAccessTime;
                    cmd.Parameters.Add(lastAccessTimeParam);
                    var lastWriteTimeParam = cmd.CreateParameter();
                    lastWriteTimeParam.Direction = ParameterDirection.Input;
                    lastWriteTimeParam.DbType = DbType.DateTime2;
                    lastWriteTimeParam.SqlDbType = SqlDbType.DateTime2;
                    lastWriteTimeParam.ParameterName = "@LastWriteTimeLocal";
                    lastWriteTimeParam.Value = lastWriteTime;
                    cmd.Parameters.Add(lastWriteTimeParam);
                    var directoryNameParam = cmd.CreateParameter();
                    directoryNameParam.Size = 255;
                    directoryNameParam.DbType = DbType.AnsiString;
                    directoryNameParam.SqlDbType = SqlDbType.VarChar;
                    directoryNameParam.ParameterName = "@DirectoryName";
                    directoryNameParam.Value = directoryName ?? (object) DBNull.Value;
                    directoryNameParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(directoryNameParam);
                    var fullFilePathParam = cmd.CreateParameter();
                    fullFilePathParam.Direction = ParameterDirection.Input;
                    fullFilePathParam.DbType = DbType.String;
                    fullFilePathParam.SqlDbType = SqlDbType.NVarChar;
                    fullFilePathParam.ParameterName = "@FullFilePath";
                    fullFilePathParam.Size = 4000;
                    fullFilePathParam.Value = fullFilePath ?? (object) DBNull.Value;
                    cmd.Parameters.Add(fullFilePathParam);
                    var fileUrlParam = cmd.CreateParameter();
                    fileUrlParam.ParameterName = "@FileUrl";
                    fileUrlParam.Direction = ParameterDirection.Input;
                    fileUrlParam.Value = fileUrl ?? (object) DBNull.Value;
                    fileUrlParam.DbType = DbType.String;
                    fileUrlParam.SqlDbType = SqlDbType.NVarChar;
                    fileUrlParam.Size = 4000;
                    cmd.Parameters.Add(fileUrlParam);
                    var byteCountParam = cmd.CreateParameter();
                    byteCountParam.Direction = ParameterDirection.Input;
                    byteCountParam.Value = byteCount;
                    byteCountParam.DbType = DbType.Int64;
                    byteCountParam.SqlDbType = SqlDbType.BigInt;
                    byteCountParam.ParameterName = "@ByteCount";
                    cmd.Parameters.Add(byteCountParam);
                    var fileTypeIdParam = cmd.CreateParameter();
                    fileTypeIdParam.Direction = ParameterDirection.Input;
                    fileTypeIdParam.Value = fileTypeId;
                    fileTypeIdParam.DbType = DbType.Byte;
                    fileTypeIdParam.SqlDbType = SqlDbType.TinyInt;
                    fileTypeIdParam.ParameterName = "@FileTypeID";
                    cmd.Parameters.Add(fileTypeIdParam);
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.Direction = ParameterDirection.Output;
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(documentIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return documentIdParam.Value as int? ?? default(int);
                });
            });

        public void MergeDocuments(DataTable dt, FileType fileType) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                var proc = fileType == FileType.Images ? "[dbo].[uspMergeImageDocuments]" :
                    fileType == FileType.Invoices ? "[dbo].[uspMergeInvoiceDocuments]" :
                    throw new Exception($"Error, could not find a valid file type for arguement {nameof(fileType)}");
                DisposableService.Using(() => new SqlCommand(proc, conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 240;
                    var documentsParam = cmd.CreateParameter();
                    documentsParam.SqlDbType = SqlDbType.Structured;
                    documentsParam.Direction = ParameterDirection.Input;
                    documentsParam.Value = dt;
                    documentsParam.TypeName = "dbo.udtDocument";
                    documentsParam.ParameterName = "@Documents";
                    cmd.Parameters.Add(documentsParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });
    }
}