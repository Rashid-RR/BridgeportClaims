﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public class ClaimImageProvider : IClaimImageProvider
    {
        public ClaimImagesDto GetClaimImages(string sortColumn, string sortDirection, int pageNumber, int pageSize) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetClaimImages]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var sortColumnParam = cmd.CreateParameter();
                    sortColumnParam.Direction = ParameterDirection.Input;
                    sortColumnParam.Value = sortColumn ?? (object) DBNull.Value;
                    sortColumnParam.ParameterName = "@SortColumn";
                    sortColumnParam.DbType = DbType.AnsiStringFixedLength;
                    sortColumnParam.Size = 50;
                    sortColumnParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(sortColumnParam);
                    var sortDirectionParam = cmd.CreateParameter();
                    sortDirectionParam.Direction = ParameterDirection.Input;
                    sortDirectionParam.Value = sortDirection ?? (object) DBNull.Value;
                    sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    sortDirectionParam.DbType = DbType.AnsiStringFixedLength;
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
                    pageSizeParam.ParameterName = "@PageNumber";
                    cmd.Parameters.Add(pageSizeParam);
                    var totalRowsParam = cmd.CreateParameter();
                    totalRowsParam.Direction = ParameterDirection.Output;
                    totalRowsParam.ParameterName = "@TotalRows";
                    totalRowsParam.DbType = DbType.Int32;
                    totalRowsParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(totalRowsParam);
                    var retVal = new ClaimImagesDto();
                    IList<ClaimImageResultDto> results = new List<ClaimImageResultDto>();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var documentIdOrdinal = reader.GetOrdinal("DocumentId");
                        var createdOrdinal = reader.GetOrdinal("Created");
                        var typeOrdinal = reader.GetOrdinal("Type");
                        var rxDateOrdinal = reader.GetOrdinal("RxDate");
                        var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
                        var fileNameOrdianl = reader.GetOrdinal("FileName");
                        while (reader.Read())
                        {
                            var result = new ClaimImageResultDto
                            {
                                Created = reader.GetDateTime(createdOrdinal),
                                DocumentId = reader.GetInt32(documentIdOrdinal),
                                FileName = reader.GetString(fileNameOrdianl),
                                RxDate = !reader.IsDBNull(rxDateOrdinal) ? reader.GetDateTime(rxDateOrdinal) : (DateTime?) null,
                                RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
                                Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty
                            };
                            results.Add(result);
                        }
                    });
                    retVal.TotalRowCount = totalRowsParam.Value as int? ?? default(int);
                    retVal.ClaimImages = results;
                    return retVal;
                });
            });
    }
}