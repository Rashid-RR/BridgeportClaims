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

        internal int InsertDocument(string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime) =>
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
                    return 1;
                });
            });
    }
}