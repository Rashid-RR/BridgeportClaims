﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public class DocumentsProvider : IDocumentsProvider
    {
        private readonly IRepository<Document> _documentRepository;

        public DocumentsProvider(IRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public IList<DocumentResultDto> GetDocumentByFileName(string fileName) =>
            _documentRepository.GetMany(x => x.FileName.Contains(fileName))?.Select(doc => new DocumentResultDto
            {
                DocumentId = doc.DocumentId,
                CreationTimeLocal = doc.CreationTimeLocal,
                Extension = doc.Extension,
                FileName = doc.FileName,
                FileSize = doc.FileSize,
                FileUrl = doc.FileUrl,
                FullFilePath = doc.FullFilePath,
                LastAccessTimeLocal = doc.LastAccessTimeLocal,
                LastWriteTimeLocal = doc.LastWriteTimeLocal
            }).ToList();

        public IList<DocumentTypeDto> GetDocumentTypes() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("SELECT [dt].[DocumentTypeID] DocumentTypeId, [dt].[TypeName] FROM [dbo].[DocumentType] AS [dt]", conn), cmd =>
                    {
                        IList<DocumentTypeDto> types = new List<DocumentTypeDto>();
                        cmd.CommandType = CommandType.Text;
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        DisposableService.Using(cmd.ExecuteReader, reader =>
                        {
                            var documentTypeIdOrdinal = reader.GetOrdinal("DocumentTypeId");
                            var typeNameOrdinal = reader.GetOrdinal("TypeName");
                            while (reader.Read())
                            {
                                var documentTypeDto = new DocumentTypeDto
                                {
                                    DocumentTypeId = reader.GetByte(documentTypeIdOrdinal),
                                    TypeName = reader.GetString(typeNameOrdinal)
                                };
                                types.Add(documentTypeDto);
                            }
                        });
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                        return types.OrderBy(x => x.TypeName).ToList();
                    });
            });

        public void ArchiveDocument(int documentId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspArchiveDocument]", conn), cmd =>
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

        public DocumentsDto GetDocuments(DateTime? date, string fileName, string sortColumn, string sortDirection, int pageNumber, int pageSize) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetDocuments]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var retVal = new DocumentsDto();
                    var dateParam = cmd.CreateParameter();
                    dateParam.Direction = ParameterDirection.Input;
                    dateParam.SqlDbType = SqlDbType.Date;
                    dateParam.DbType = DbType.Date;
                    dateParam.Value = date ?? (object) DBNull.Value;
                    dateParam.ParameterName = "@Date";
                    cmd.Parameters.Add(dateParam);
                    var fileNameParam = cmd.CreateParameter();
                    fileNameParam.Direction = ParameterDirection.Input;
                    fileNameParam.SqlDbType = SqlDbType.VarChar;
                    fileNameParam.Size = 1000;
                    fileNameParam.DbType = DbType.AnsiString;
                    fileNameParam.Value = fileName ?? (object) DBNull.Value;
                    fileNameParam.ParameterName = "@FileName";
                    cmd.Parameters.Add(fileNameParam);
                    var sortColumnParam = cmd.CreateParameter();
                    sortColumnParam.Direction = ParameterDirection.Input;
                    sortColumnParam.SqlDbType = SqlDbType.VarChar;
                    sortColumnParam.Size = 50;
                    sortColumnParam.DbType = DbType.AnsiString;
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
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    retVal.DocumentTypes = GetDocumentTypes();
                    return retVal;
                });
            });
    }
}