using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.FileWatcherBusiness.Disposable;
using cm = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.DAL
{
    internal class ImageDataProvider
    {
        private readonly string _dbConnStr = cm.GetDbConnStr();

        internal void DeleteDocument(int documentId) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentDelete]", conn), cmd =>
                {
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
                    conn.Close();
                });
            });

        internal void UpdateDocument(int documentId, string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
            DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentUpdate]", conn), cmd =>
                {
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Value = documentId;
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    documentIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(documentIdParam);
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.DbType = DbType.AnsiStringFixedLength;
                    fileNameParam.SqlDbType = SqlDbType.VarChar;
                    fileNameParam.ParameterName = "@FileName";
                    fileNameParam.Size = 1000;
                    fileNameParam.Value = fileName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fileNameParam);
                    var extensionParam = cmd.CreateParameter();
                    extensionParam.Direction = ParameterDirection.Input;
                    extensionParam.DbType = DbType.AnsiStringFixedLength;
                    extensionParam.SqlDbType = SqlDbType.VarChar;
                    extensionParam.ParameterName = "@Extension";
                    extensionParam.Value = extension ?? (object)DBNull.Value;
                    extensionParam.Size = 50;
                    cmd.Parameters.Add(extensionParam);
                    var fileSizeParam = cmd.CreateParameter();
                    fileSizeParam.Direction = ParameterDirection.Input;
                    fileSizeParam.DbType = DbType.AnsiStringFixedLength;
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
                    directoryNameParam.DbType = DbType.AnsiStringFixedLength;
                    directoryNameParam.SqlDbType = SqlDbType.VarChar;
                    directoryNameParam.ParameterName = "@DirectoryName";
                    directoryNameParam.Value = directoryName ?? (object)DBNull.Value;
                    directoryNameParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(directoryNameParam);
                    var fullFilePathParam = cmd.CreateParameter();
                    fullFilePathParam.Direction = ParameterDirection.Input;
                    fullFilePathParam.DbType = DbType.StringFixedLength;
                    fullFilePathParam.SqlDbType = SqlDbType.NVarChar;
                    fullFilePathParam.ParameterName = "@FullFilePath";
                    fullFilePathParam.Size = 4000;
                    fullFilePathParam.Value = fullFilePath ?? (object)DBNull.Value;
                    cmd.Parameters.Add(fullFilePathParam);
                    var fileUrlParam = cmd.CreateParameter();
                    fileUrlParam.ParameterName = "@FileUrl";
                    fileUrlParam.Direction = ParameterDirection.Input;
                    fileUrlParam.Value = fileUrl ?? (object)DBNull.Value;
                    fileUrlParam.DbType = DbType.StringFixedLength;
                    fileUrlParam.SqlDbType = SqlDbType.NVarChar;
                    fileUrlParam.Size = 4000;
                    cmd.Parameters.Add(fileUrlParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                });
            });

        internal int InsertDocument(string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
                DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspDocumentInsert]", conn), cmd =>
                {
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.DbType = DbType.AnsiStringFixedLength;
                    fileNameParam.SqlDbType = SqlDbType.VarChar;
                    fileNameParam.ParameterName = "@FileName";
                    fileNameParam.Size = 1000;
                    fileNameParam.Value = fileName ?? (object) DBNull.Value;
                    cmd.Parameters.Add(fileNameParam);
                    var extensionParam = cmd.CreateParameter();
                    extensionParam.Direction = ParameterDirection.Input;
                    extensionParam.DbType = DbType.AnsiStringFixedLength;
                    extensionParam.SqlDbType = SqlDbType.VarChar;
                    extensionParam.ParameterName = "@Extension";
                    extensionParam.Value = extension ?? (object) DBNull.Value;
                    extensionParam.Size = 50;
                    cmd.Parameters.Add(extensionParam);
                    var fileSizeParam = cmd.CreateParameter();
                    fileSizeParam.Direction = ParameterDirection.Input;
                    fileSizeParam.DbType = DbType.AnsiStringFixedLength;
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
                    directoryNameParam.DbType = DbType.AnsiStringFixedLength;
                    directoryNameParam.SqlDbType = SqlDbType.VarChar;
                    directoryNameParam.ParameterName = "@DirectoryName";
                    directoryNameParam.Value = directoryName ?? (object) DBNull.Value;
                    directoryNameParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(directoryNameParam);
                    var fullFilePathParam = cmd.CreateParameter();
                    fullFilePathParam.Direction = ParameterDirection.Input;
                    fullFilePathParam.DbType = DbType.StringFixedLength;
                    fullFilePathParam.SqlDbType = SqlDbType.NVarChar;
                    fullFilePathParam.ParameterName = "@FullFilePath";
                    fullFilePathParam.Size = 4000;
                    fullFilePathParam.Value = fullFilePath ?? (object) DBNull.Value;
                    cmd.Parameters.Add(fullFilePathParam);
                    var fileUrlParam = cmd.CreateParameter();
                    fileUrlParam.ParameterName = "@FileUrl";
                    fileUrlParam.Direction = ParameterDirection.Input;
                    fileUrlParam.Value = fileUrl ?? (object) DBNull.Value;
                    fileUrlParam.DbType = DbType.StringFixedLength;
                    fileUrlParam.SqlDbType = SqlDbType.NVarChar;
                    fileUrlParam.Size = 4000;
                    cmd.Parameters.Add(fileUrlParam);
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.ParameterName = "@DocumentID";
                    documentIdParam.Direction = ParameterDirection.Output;
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(documentIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return documentIdParam.Value as int? ?? default(int);
                });
            });
    }
}