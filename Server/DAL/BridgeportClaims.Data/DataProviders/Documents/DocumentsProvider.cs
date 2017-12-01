using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public class DocumentsProvider : IDocumentsProvider
    {
        public DocumentsDto GetDocuments(string sortColumn, string sortDirection, int pageNumber, int pageSize) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var retVal = new DocumentsDto();
                    var sortColumnParam = cmd.CreateParameter();
                    sortColumnParam.Direction = ParameterDirection.Input;
                    sortColumnParam.SqlDbType = SqlDbType.VarChar;
                    sortColumnParam.Size = 50;
                    sortColumnParam.DbType = DbType.AnsiStringFixedLength;
                    sortColumnParam.ParameterName = "@SortColumn";
                    sortColumnParam.Value = sortColumn ?? (object) DBNull.Value;
                    cmd.Parameters.Add(sortColumnParam);
                    var sortDirectionParam = cmd.CreateParameter();
                    sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.DbType = DbType.String;
                    sortDirectionParam.Value = sortDirection ?? (object) DBNull.Value;
                    sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    sortDirectionParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(sortDirectionParam);
                    var pageParam = cmd.CreateParameter();
                    pageParam.ParameterName = "@PageNumber";
                    pageParam.DbType = DbType.Int32;
                    pageParam.Value = pageNumber;
                    pageParam.Direction = ParameterDirection.Input;
                    pageParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(pageParam);
                    var pageSizeParam = cmd.CreateParameter();
                    pageSizeParam.ParameterName = "@PageSize";
                    pageSizeParam.Value = pageSize;
                    pageSizeParam.DbType = DbType.Int32;
                    pageSizeParam.SqlDbType = SqlDbType.Int;
                    pageSizeParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(pageSizeParam);
                    var totalRowsParam = cmd.CreateParameter();
                    totalRowsParam.ParameterName = "@TotalRows";
                    totalRowsParam.DbType = DbType.Int32;
                    totalRowsParam.SqlDbType = SqlDbType.Int;
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalRowsParam);
                    IList<DocumentResultDto> retValResults = new List<DocumentResultDto>();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var documentIdOrdinal = reader.GetOrdinal("DocumentId");
                        var fileNameOrdinal = reader.GetOrdinal("FileName");
                        var extensionOrdinal = reader.GetOrdinal("Extension");
                        var fileSizeOrdinal = reader.GetOrdinal("FileSize");
                        var creationTimeOrdinal = reader.GetOrdinal("CreationTimeLocal");
                        var lastAccessTimeOrdinal = reader.GetOrdinal("LastAccessTimeLocal");
                        var lastWriteTimeOrdinal = reader.GetOrdinal("LastWriteTimeLocal");
                        var fullFilePathOrdinal = reader.GetOrdinal("FullFilePath");
                        var fileUrlOrdinal = reader.GetOrdinal("FileUrl");
                        while (reader.Read())
                        {
                            var result = new DocumentResultDto
                            {
                                CreationTimeLocal = reader.GetDateTime(creationTimeOrdinal),
                                DocumentId = reader.GetInt32(documentIdOrdinal),
                                Extension = !reader.IsDBNull(extensionOrdinal) ? reader.GetString(extensionOrdinal) : string.Empty,
                                FileName = !reader.IsDBNull(fileNameOrdinal) ? reader.GetString(fileNameOrdinal) : string.Empty,
                                FileSize = !reader.IsDBNull(fileSizeOrdinal) ? reader.GetString(fileSizeOrdinal) : string.Empty,
                                FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : string.Empty,
                                FullFilePath = !reader.IsDBNull(fullFilePathOrdinal) ? reader.GetString(fullFilePathOrdinal) : string.Empty,
                                LastAccessTimeLocal = reader.GetDateTime(lastAccessTimeOrdinal),
                                LastWriteTimeLocal = reader.GetDateTime(lastWriteTimeOrdinal)
                            };
                            retValResults.Add(result);
                        }
                    });
                    retVal.DocumentResults = retValResults;
                    retVal.TotalRowCount = totalRowsParam.Value as int? ?? default(int);
                    conn.Close();
                    return retVal;
                });
            });
    }
}