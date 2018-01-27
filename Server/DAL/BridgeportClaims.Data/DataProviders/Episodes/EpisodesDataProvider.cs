﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	
	public class EpisodesDataProvider : IEpisodesDataProvider
	{
		private readonly IRepository<EpisodeType> _episodeTypeRepository;

		public EpisodesDataProvider(IRepository<EpisodeType> episodeTypeRepository)
		{
		    _episodeTypeRepository = episodeTypeRepository;
		}

	    public IList<EpisodeResultsDto> GetEpisodes(bool resolved, string sortColumn, string sortDirection, int pageNumber) =>
	        DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
	        {
	            return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetEpisodes]", conn), cmd =>
	            {
	                cmd.CommandType = CommandType.StoredProcedure;
	                var resolvedParam = cmd.CreateParameter();
	                resolvedParam.Direction = ParameterDirection.Input;
	                resolvedParam.Value = resolved;
	                resolvedParam.ParameterName = "@Resolved";
	                resolvedParam.DbType = DbType.Boolean;
	                resolvedParam.SqlDbType = SqlDbType.Bit;
	                cmd.Parameters.Add(resolvedParam);
	                var sortColumnParam = cmd.CreateParameter();
	                sortColumnParam.Value = sortColumn ?? (object) DBNull.Value;
                    sortColumnParam.Direction = ParameterDirection.Input;
                    sortColumnParam.DbType= DbType.AnsiString;
	                sortColumnParam.Size = 50;
	                sortColumnParam.ParameterName = "@SortColumn";
	                sortColumnParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(sortColumnParam);
	                var sortDirectionParam = cmd.CreateParameter();
	                sortDirectionParam.Value = sortDirection ?? (object) DBNull.Value;
	                sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.DbType = DbType.AnsiString;
	                sortDirectionParam.Size = 5;
                    sortDirectionParam.Direction = ParameterDirection.Input;
	                sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(sortDirectionParam);
	                var pageNumberParam = cmd.CreateParameter();
	                pageNumberParam.Value = default(int) == pageNumber ? 5000 : pageNumber;
	                pageNumberParam.DbType = DbType.Int32;
                    pageNumberParam.SqlDbType = SqlDbType.Int;
                    pageNumberParam.Direction = ParameterDirection.Input;
	                pageNumberParam.ParameterName = "@PageNumber";
                    cmd.Parameters.Add(pageNumberParam);
	                var totalPageSizeParam = cmd.CreateParameter();
	                totalPageSizeParam.ParameterName = "@TotalPageSize";
                    totalPageSizeParam.Direction = ParameterDirection.Output;
                    totalPageSizeParam.DbType = DbType.Int32;
                    totalPageSizeParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(totalPageSizeParam);
                    var retVal = new List<EpisodeResultsDto>();
	                if (conn.State != ConnectionState.Open)
	                    conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var episodeIdOrdinal = reader.GetOrdinal("EpisodeId");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        var createdOrdinal = reader.GetOrdinal("Created");
                        var patientNameOrdinal = reader.GetOrdinal("Patient");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var typeOrdinal = reader.GetOrdinal("Type");
                        var pharmacyOrdinal = reader.GetOrdinal("Pharmacy");
                        var carrierOrdinal = reader.GetOrdinal("Carrier");
                        var episodeNoteOrdinal = reader.GetOrdinal("EpisodeNote");
                        while (reader.Read())
                        {
                            var result = new EpisodeResultsDto
                            {
                                EpisodeId = !reader.IsDBNull(episodeIdOrdinal) ? reader.GetInt32(episodeIdOrdinal) : throw new Exception("Error, there cannot be a null Episode ID"),
                                Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
                                Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal) : (DateTime?) null,
                                PatientName = !reader.IsDBNull(patientNameOrdinal) ? reader.GetString(patientNameOrdinal) : string.Empty,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : string.Empty,
                                Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
                                Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
                                Carrier = !reader.IsDBNull(carrierOrdinal) ? reader.GetString(carrierOrdinal) : string.Empty,
                                EpisodeNote = !reader.IsDBNull(episodeNoteOrdinal) ? reader.GetString(episodeNoteOrdinal) : string.Empty
                            };
                            retVal.Add(result);
                        }
                    });
	                if (conn.State != ConnectionState.Closed)
	                    conn.Close();
	                return retVal;
	            });
	        });

		public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId)
		{
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				DisposableService.Using(() => new SqlCommand("uspSaveEpisode", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;
					var epId = new SqlParameter
					{
						ParameterName = "@EpisodeID",
						Value = (object) episodeId ?? DBNull.Value,
						DbType = DbType.Int32,
						SqlDbType = SqlDbType.Int
					};
					var clId = new SqlParameter
					{
						ParameterName = "@ClaimID",
						Value = claimId,
						DbType = DbType.Int32,
						SqlDbType = SqlDbType.Int
					};
					var cd = new SqlParameter
					{
						ParameterName = "@CreatedDateUTC",
						Value = DateTime.UtcNow,
						DbType = DbType.DateTime2,
						SqlDbType = SqlDbType.DateTime2
					};
					var uId = new SqlParameter
					{
						ParameterName = "@AssignedUserID",
						Value = by,
						DbType = DbType.String,
						SqlDbType = SqlDbType.NVarChar
					};
					var note = new SqlParameter
					{
						ParameterName = "@Note",
						Value = (object) noteText ?? DBNull.Value,
						DbType = DbType.String,
						SqlDbType = SqlDbType.VarChar
					};
					var etId = new SqlParameter
					{
						ParameterName = "@EpisodeTypeID",
						Value = (object) episodeTypeId ?? DBNull.Value,
						DbType = DbType.Int32,
						SqlDbType = SqlDbType.Int
					};
					cmd.Parameters.Add(epId);
					cmd.Parameters.Add(clId);
					cmd.Parameters.Add(cd);
					cmd.Parameters.Add(uId);
					cmd.Parameters.Add(note);
					cmd.Parameters.Add(etId);
				    if (conn.State != ConnectionState.Open)
				        conn.Open();
                    cmd.ExecuteNonQuery();
				});
			});
		}

		public IList<EpisodeTypeDto> GetEpisodeTypes() => _episodeTypeRepository.GetAll()
		    .Select(e => new EpisodeTypeDto
		    {
		        EpisodeTypeId = e.EpisodeTypeId,
		        EpisodeTypeName = e.TypeName
		    }).OrderBy(x => x.EpisodeTypeName).ToList();
	}
}