﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public class ClaimImageProvider : IClaimImageProvider
    {
        public ClaimImagesDto GetClaimImages(int claimId, string sortColumn, string sortDirection, int pageNumber, int pageSize) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetClaimImages]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var claimIdParam = cmd.CreateParameter();
                    claimIdParam.Value = claimId;
                    claimIdParam.ParameterName = "@ClaimID";
                    claimIdParam.Direction = ParameterDirection.Input;
                    claimIdParam.DbType = DbType.Int32;
                    claimIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(claimIdParam);
                    var sortColumnParam = cmd.CreateParameter();
                    sortColumnParam.Direction = ParameterDirection.Input;
                    sortColumnParam.Value = sortColumn ?? (object) DBNull.Value;
                    sortColumnParam.ParameterName = "@SortColumn";
                    sortColumnParam.DbType = DbType.AnsiString;
                    sortColumnParam.Size = 50;
                    sortColumnParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(sortColumnParam);
                    var sortDirectionParam = cmd.CreateParameter();
                    sortDirectionParam.Direction = ParameterDirection.Input;
                    sortDirectionParam.Value = sortDirection ?? (object) DBNull.Value;
                    sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    sortDirectionParam.DbType = DbType.AnsiString;
                    sortDirectionParam.Size = 5;
                    cmd.Parameters.Add(sortDirectionParam);
                    var pageNumberParam = cmd.CreateParameter();
                    pageNumberParam.Direction = ParameterDirection.Input;
                    pageNumberParam.Value = pageNumber;
                    pageNumberParam.ParameterName = "@PageNumber";
                    pageNumberParam.DbType = DbType.Int32;
                    pageNumberParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(pageNumberParam);
                    var pageSizeParam = cmd.CreateParameter();
                    pageSizeParam.Direction = ParameterDirection.Input;
                    pageSizeParam.Value = pageSize;
                    pageSizeParam.DbType = DbType.Int32;
                    pageSizeParam.SqlDbType = SqlDbType.Int;
                    pageSizeParam.ParameterName = "@PageSize";
                    cmd.Parameters.Add(pageSizeParam);
                    var totalRowsParam = cmd.CreateParameter();
                    totalRowsParam.Direction = ParameterDirection.Output;
                    totalRowsParam.ParameterName = "@TotalRows";
                    totalRowsParam.DbType = DbType.Int32;
                    totalRowsParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(totalRowsParam);
                    var retVal = new ClaimImagesDto();
                    IList<ClaimImageResultDto> results = new List<ClaimImageResultDto>();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var documentIdOrdinal = reader.GetOrdinal("DocumentId");
                        var createdOrdinal = reader.GetOrdinal("Created");
                        var typeOrdinal = reader.GetOrdinal("Type");
                        var rxDateOrdinal = reader.GetOrdinal("RxDate");
                        var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
                        var invoiceNumberOrdinal = reader.GetOrdinal("InvoiceNumber");
                        var injuryDateOrdinal = reader.GetOrdinal("InjuryDate");
                        var attorneyNameOrdinal = reader.GetOrdinal("AttorneyName");
                        var fileNameOrdinal = reader.GetOrdinal("FileName");
                        var noteCountOrdinal = reader.GetOrdinal("NoteCount");
                        var episodeIdOrdinal = reader.GetOrdinal("EpisodeId");
                        var fileUrlOrdinal = reader.GetOrdinal("FileUrl");
                        var fileDateOrdinal = reader.GetOrdinal("FileDate");
                        while (reader.Read())
                        {
                            var result = new ClaimImageResultDto
                            {
                                Created = reader.GetDateTime(createdOrdinal),
                                DocumentId = reader.GetInt32(documentIdOrdinal),
                                FileName = reader.GetString(fileNameOrdinal),
                                RxDate = !reader.IsDBNull(rxDateOrdinal) ? reader.GetDateTime(rxDateOrdinal) : (DateTime?) null,
                                RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
                                InvoiceNumber = !reader.IsDBNull(invoiceNumberOrdinal) ? reader.GetString(invoiceNumberOrdinal) : string.Empty,
                                InjuryDate = !reader.IsDBNull(injuryDateOrdinal) ? reader.GetDateTime(injuryDateOrdinal) : (DateTime?) null,
                                AttorneyName = !reader.IsDBNull(attorneyNameOrdinal) ? reader.GetString(attorneyNameOrdinal) : string.Empty,
                                Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
                                NoteCount = !reader.IsDBNull(noteCountOrdinal) ? reader.GetInt32(noteCountOrdinal) : default,
                                EpisodeId = !reader.IsDBNull(episodeIdOrdinal) ? reader.GetInt32(episodeIdOrdinal) : default,
                                FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : string.Empty,
                                FileDate = !reader.IsDBNull(fileDateOrdinal) ? reader.GetDateTime(fileDateOrdinal) : (DateTime?) null
                            };
                            results.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    retVal.TotalRowCount = totalRowsParam.Value as int? ?? default;
                    retVal.ClaimImages = results;
                    return retVal;
                });
            });

        public void UpdateDocumentIndex(int documentId, int claimId, byte documentTypeId, DateTime? rxDate, string rxNumber,
            string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspDocumentIndexUpdate]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@DocumentID", documentId, DbType.Int32);
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@DocumentTypeID", documentTypeId, DbType.Byte);
                ps.Add("@RxDate", rxDate, DbType.DateTime2);
                ps.Add("@RxNumber", rxNumber, DbType.AnsiString, size: 100);
                ps.Add("@InvoiceNumber", invoiceNumber, DbType.AnsiString, size: 100);
                ps.Add("@InjuryDate", injuryDate, DbType.DateTime2);
                ps.Add("@AttorneyName", attorneyName, DbType.AnsiString, size: 255);
                ps.Add("@IndexedByUserID", indexedByUserId, DbType.String, size: 128);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });

        public void ReindexDocumentImage(int documentId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var query = $@"DECLARE @DocumentID INT = {documentId};
                               DELETE dbo.DocumentIndex WHERE DocumentID = @DocumentID;";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(query, commandType: CommandType.Text);
            });
    }
}