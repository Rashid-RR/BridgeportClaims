﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimSearches
{
    public class ClaimSearchProvider : IClaimSearchProvider
    {
        public IList<DocumentClaimSearchResultDto> GetDocumentClaimSearchResults(string searchText, bool exactMatch, string delimiter) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspClaimTextSearch]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    IList<DocumentClaimSearchResultDto> retVal = new List<DocumentClaimSearchResultDto>();
                    var searchTextParam = cmd.CreateParameter();
                    searchTextParam.Value = searchText ?? (object) DBNull.Value;
                    searchTextParam.DbType = DbType.AnsiStringFixedLength;
                    searchTextParam.SqlDbType = SqlDbType.VarChar;
                    searchTextParam.Size = 800;
                    searchTextParam.ParameterName = "@SearchText";
                    cmd.Parameters.Add(searchTextParam);
                    var exactMatchParam = cmd.CreateParameter();
                    exactMatchParam.Direction = ParameterDirection.Input;
                    exactMatchParam.DbType = DbType.Boolean;
                    exactMatchParam.SqlDbType = SqlDbType.Bit;
                    exactMatchParam.Value = exactMatch;
                    exactMatchParam.ParameterName = "@ExactMatch";
                    cmd.Parameters.Add(exactMatchParam);
                    var delimiterParam = cmd.CreateParameter();
                    delimiterParam.Value = delimiter;
                    delimiterParam.ParameterName = "@Delimiter";
                    delimiterParam.Direction = ParameterDirection.Input;
                    delimiterParam.DbType = DbType.AnsiStringFixedLength;
                    delimiterParam.SqlDbType = SqlDbType.Char;
                    delimiterParam.Size = 1;
                    cmd.Parameters.Add(delimiterParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                        var lastNameOrdinal = reader.GetOrdinal("LastName");
                        var firstNameOrdinal = reader.GetOrdinal("FirstName");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var groupNumberOrdinal = reader.GetOrdinal("GroupNumber");
                        while (reader.Read())
                        {
                            var result = new DocumentClaimSearchResultDto
                            {
                                ClaimId = reader.GetInt32(claimIdOrdinal),
                                ClaimNumber = reader.GetString(claimNumberOrdinal),
                                FirstName = reader.GetString(firstNameOrdinal),
                                GroupNumber = reader.GetString(groupNumberOrdinal),
                                LastName = reader.GetString(lastNameOrdinal)
                            };
                            retVal.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal;
                });
            });
    }
}