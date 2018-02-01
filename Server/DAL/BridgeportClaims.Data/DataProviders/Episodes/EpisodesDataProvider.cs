﻿using System;
using System.Collections.Generic;
using System.CodeDom;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
	    private readonly IRepository<Episode> _episodeRepository;
	    private readonly IRepository<AspNetUsers> _usersRepository;
	    private readonly IRepository<EpisodeCategory> _episodeCategoryRepository;
	    private readonly IRepository<Claim> _claimRepository;
	    private readonly IRepository<Pharmacy> _pharmacyRepository;

        public EpisodesDataProvider(
            IRepository<EpisodeType> episodeTypeRepository, 
            IRepository<AspNetUsers> usersRepository, 
            IRepository<Episode> episodeRepository, 
            IRepository<EpisodeCategory> episodeCategoryRepository, 
            IRepository<Claim> claimRepository, 
            IRepository<Pharmacy> pharmacyRepository)
		{
		    _episodeTypeRepository = episodeTypeRepository;
		    _usersRepository = usersRepository;
		    _episodeRepository = episodeRepository;
		    _episodeCategoryRepository = episodeCategoryRepository;
		    _claimRepository = claimRepository;
		    _pharmacyRepository = pharmacyRepository;
		}

	    public EpisodesDto GetEpisodes(DateTime? startDate, DateTime? endDate, bool resolved, string ownerId,
            int? episodeCategoryId, int? episodeTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize) =>
	        DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
	        {
	            return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetEpisodes]", conn), cmd =>
	            {
	                cmd.CommandType = CommandType.StoredProcedure;
	                var startDateParam = cmd.CreateParameter();
	                startDateParam.Value = startDate ?? (object) DBNull.Value;
	                startDateParam.ParameterName = "@StartDate";
	                startDateParam.DbType = DbType.Date;
                    startDateParam.SqlDbType = SqlDbType.Date;
                    startDateParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(startDateParam);
	                var endDateParam = cmd.CreateParameter();
	                endDateParam.Value = endDate ?? (object) DBNull.Value;
                    endDateParam.Direction = ParameterDirection.Input;
                    endDateParam.DbType = DbType.Date;
	                endDateParam.SqlDbType = SqlDbType.Date;
	                endDateParam.ParameterName = "@EndDate";
                    cmd.Parameters.Add(endDateParam);
	                var resolvedParam = cmd.CreateParameter();
	                resolvedParam.Direction = ParameterDirection.Input;
	                resolvedParam.Value = resolved;
	                resolvedParam.ParameterName = "@Resolved";
	                resolvedParam.DbType = DbType.Boolean;
	                resolvedParam.SqlDbType = SqlDbType.Bit;
	                cmd.Parameters.Add(resolvedParam);
	                var ownerIdParam = cmd.CreateParameter();
	                ownerIdParam.Value = ownerId ?? (object) DBNull.Value;
                    ownerIdParam.Direction = ParameterDirection.Input;
                    ownerIdParam.DbType = DbType.String;
	                ownerIdParam.Size = 128;
	                ownerIdParam.SqlDbType = SqlDbType.NVarChar;
	                ownerIdParam.ParameterName = "@OwnerID";
                    cmd.Parameters.Add(ownerIdParam);
	                var episodeCategoryIdParam = cmd.CreateParameter();
	                episodeCategoryIdParam.Direction = ParameterDirection.Input;
	                episodeCategoryIdParam.Value = episodeCategoryId ?? (object) DBNull.Value;
                    episodeCategoryIdParam.DbType = DbType.Int32;
                    episodeCategoryIdParam.SqlDbType = SqlDbType.Int;
	                episodeCategoryIdParam.ParameterName = "@EpisodeCategoryID";
                    cmd.Parameters.Add(episodeCategoryIdParam);
	                var episodeTypeIdParam = cmd.CreateParameter();
	                episodeTypeIdParam.Direction = ParameterDirection.Input;
	                episodeTypeIdParam.Value = episodeCategoryId ?? (object)DBNull.Value;
	                episodeTypeIdParam.DbType = DbType.Int32;
	                episodeTypeIdParam.SqlDbType = SqlDbType.Int;
	                episodeTypeIdParam.ParameterName = "@EpisodeTypeID";
	                cmd.Parameters.Add(episodeTypeIdParam);
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
	                var pageSizeParam = cmd.CreateParameter();
	                pageSizeParam.DbType = DbType.Int32;
                    pageSizeParam.SqlDbType = SqlDbType.BigInt;
                    pageSizeParam.Direction = ParameterDirection.Input;
	                pageSizeParam.Value = pageSize;
	                pageSizeParam.ParameterName = "@PageSize";
                    cmd.Parameters.Add(pageSizeParam);
	                var totalPageSizeParam = cmd.CreateParameter();
	                totalPageSizeParam.ParameterName = "@TotalPageSize";
                    totalPageSizeParam.Direction = ParameterDirection.Output;
                    totalPageSizeParam.DbType = DbType.Int32;
                    totalPageSizeParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(totalPageSizeParam);
                    var list = new List<EpisodeResultsDto>();
	                var retVal = new EpisodesDto();
	                if (conn.State != ConnectionState.Open)
	                    conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var episodeIdOrdinal = reader.GetOrdinal("EpisodeId");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        var createdOrdinal = reader.GetOrdinal("Created");
                        var patientNameOrdinal = reader.GetOrdinal("PatientName");
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
                                Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal).ToLocalTime() : (DateTime?) null,
                                PatientName = !reader.IsDBNull(patientNameOrdinal) ? reader.GetString(patientNameOrdinal) : string.Empty,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : string.Empty,
                                Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
                                Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
                                Carrier = !reader.IsDBNull(carrierOrdinal) ? reader.GetString(carrierOrdinal) : string.Empty,
                                EpisodeNote = !reader.IsDBNull(episodeNoteOrdinal) ? reader.GetString(episodeNoteOrdinal) : string.Empty
                            };
                            list.Add(result);
                        }
                    });
	                if (conn.State != ConnectionState.Closed)
	                    conn.Close();
	                retVal.EpisodeResults = list;
	                retVal.TotalRowCount = totalPageSizeParam.Value as int? ?? default(int);
	                return retVal;
	            });
	        });

		public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId)
		{
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				DisposableService.Using(() => new SqlCommand("dbo.uspSaveEpisode", conn), cmd =>
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

		public IList<EpisodeTypeDto> GetEpisodeTypes() => _episodeTypeRepository.GetAll()?
		    .Select(e => new EpisodeTypeDto
		    {
		        EpisodeTypeId = e.EpisodeTypeId,
		        EpisodeTypeName = e.TypeName
		    }).OrderBy(x => x.EpisodeTypeName).ToList();

	    public void ResolveEpisode(int episodeId, string modifiedByUserId)
	    {
	        var episodeEntity = _episodeRepository.Get(episodeId);
	        if (null == episodeEntity)
	            throw new Exception($"Not not find Episode with ID {episodeId}");
	        var now = DateTime.UtcNow;
	        var user = _usersRepository.Get(modifiedByUserId);
	        episodeEntity.UpdatedOnUtc = now;
	        episodeEntity.ModifiedByUser = user;
	        episodeEntity.ResolvedDateUtc = now;
	        _episodeRepository.Save(episodeEntity);
	    }

	    public void SaveNewEpisode(int claimId, int? episodeTypeId, string pharmacyNabp, string rxNumber, string episodeText, string userId)
	    {
	        var cat = _episodeCategoryRepository?.GetMany(x => x.Code == "CALL")?.SingleOrDefault();
            if (null == cat)
                throw new Exception("Error. Could not find an Episode Category for Code 'CALL'");
	        var user = _usersRepository.Get(userId);
            var entity = new Episode
	        {
	            EpisodeCategory = cat,
                UpdatedOnUtc = DateTime.UtcNow,
                AssignedUser = user,
                RxNumber = rxNumber,
                EpisodeType = _episodeTypeRepository.Get(episodeTypeId),
                Claim = _claimRepository.Get(claimId),
                Pharmacy = _pharmacyRepository.Get(pharmacyNabp),
                Note = episodeText,
                ModifiedByUser = user
            };
            _episodeRepository.Save(entity);
	    }
	}
}