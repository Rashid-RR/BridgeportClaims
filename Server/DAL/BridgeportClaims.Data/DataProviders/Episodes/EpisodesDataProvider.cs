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

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	
	public class EpisodesDataProvider : IEpisodesDataProvider
	{
		private readonly Lazy<IRepository<EpisodeType>> _episodeTypeRepository;
		private readonly Lazy<IRepository<EpisodeNote>> _episodeNoteRepository;
		private readonly Lazy<IRepository<Episode>> _episodeRepository;
		private readonly Lazy<IRepository<AspNetUsers>> _usersRepository;

		public EpisodesDataProvider(
		    Lazy<IRepository<EpisodeType>> episodeTypeRepository,
		    Lazy<IRepository<AspNetUsers>> usersRepository,
		    Lazy<IRepository<Episode>> episodeRepository,
		    Lazy<IRepository<EpisodeNote>> episodeNoteRepository)
		{
			_episodeTypeRepository = episodeTypeRepository;
			_usersRepository = usersRepository;
			_episodeRepository = episodeRepository;
			_episodeNoteRepository = episodeNoteRepository;
		}

		/// <summary>
		/// Calls a stored proc responsible for inserting a new Episode depending on the Document Type that was chosen.
		/// </summary>
		/// <param name="claimId"></param>
		/// <param name="userId"></param>
		/// <param name="documentId"></param>
		/// <param name="documentTypeId"></param>
		/// <param name="rxNumber"></param>
		public bool CreateImageCategoryEpisode(byte documentTypeId, int claimId, string rxNumber, string userId, int documentId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("[dbo].[uspCreateImageCategoryEpisode]", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;
					var documentTypeIdParam = cmd.CreateParameter();
					documentTypeIdParam.Direction = ParameterDirection.Input;
					documentTypeIdParam.DbType = DbType.Byte;
					documentTypeIdParam.SqlDbType = SqlDbType.TinyInt;
					documentTypeIdParam.ParameterName = "@DocumentTypeID";
					documentTypeIdParam.Value = documentTypeId;
					cmd.Parameters.Add(documentTypeIdParam);
					var claimIdParam = cmd.CreateParameter();
					claimIdParam.Direction = ParameterDirection.Input;
					claimIdParam.DbType = DbType.Int32;
					claimIdParam.SqlDbType = SqlDbType.Int;
					claimIdParam.ParameterName = "@ClaimID";
					claimIdParam.Value = claimId;
					cmd.Parameters.Add(claimIdParam);
					var rxNumberParam = cmd.CreateParameter();
					rxNumberParam.Value = rxNumber ?? (object) DBNull.Value;
					rxNumberParam.Size = 100;
					rxNumberParam.ParameterName = "@RxNumber";
					rxNumberParam.Direction = ParameterDirection.Input;
					rxNumberParam.DbType = DbType.AnsiString;
					rxNumberParam.SqlDbType = SqlDbType.VarChar;
					cmd.Parameters.Add(rxNumberParam);
					var userIdParam = cmd.CreateParameter();
					userIdParam.Value = userId ?? throw new ArgumentNullException(nameof(userId));
					userIdParam.DbType = DbType.String;
					userIdParam.SqlDbType = SqlDbType.NVarChar;
					userIdParam.Size = 128;
					userIdParam.ParameterName = "@UserID";
					userIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(userIdParam);
					var documentIdParam = cmd.CreateParameter();
					documentIdParam.Value = default(int) == documentId ? throw new Exception($"Error, document Id {documentId} is not a valid document.") : documentId;
					documentIdParam.DbType = DbType.Int32;
					documentIdParam.SqlDbType = SqlDbType.Int;
					documentIdParam.ParameterName = "@DocumentID";
					documentIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(documentIdParam);
					var episodeCreatedParam = cmd.CreateParameter();
					episodeCreatedParam.ParameterName = "@EpisodeCreated";
					episodeCreatedParam.DbType = DbType.Boolean;
					episodeCreatedParam.SqlDbType = SqlDbType.Bit;
					episodeCreatedParam.Direction = ParameterDirection.Output;
					cmd.Parameters.Add(episodeCreatedParam);
					if (conn.State != ConnectionState.Open)
						conn.Open();
					cmd.ExecuteNonQuery();
					if (conn.State != ConnectionState.Closed)
						conn.Close();
					var retVal = episodeCreatedParam.Value as bool?;
					return retVal ?? throw new Exception("Error. The value of the output parameter was null");
				});
			});

		public EpisodesDto GetEpisodes(DateTime? startDate, DateTime? endDate, bool resolved, string ownerId, int? episodeCategoryId, 
			byte? episodeTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize, string userId) =>
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
					episodeTypeIdParam.Value = episodeTypeId ?? (object)DBNull.Value;
					episodeTypeIdParam.DbType = DbType.Byte;
					episodeTypeIdParam.SqlDbType = SqlDbType.TinyInt;
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
					var userIdParam = cmd.CreateParameter();
					userIdParam.Value = userId ?? (object)DBNull.Value;
					userIdParam.Direction = ParameterDirection.Input;
					userIdParam.DbType = DbType.String;
					userIdParam.Size = 128;
					userIdParam.SqlDbType = SqlDbType.NVarChar;
					userIdParam.ParameterName = "@UserID";
					cmd.Parameters.Add(userIdParam);
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
						var episodeNoteCountOrdinal = reader.GetOrdinal("EpisodeNoteCount");
						var fileUrlOrdinal = reader.GetOrdinal("FileUrl");
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
								EpisodeNoteCount = !reader.IsDBNull(episodeNoteCountOrdinal) ? reader.GetInt32(episodeNoteCountOrdinal) : default (int),
								FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : string.Empty
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
		
		public IList<EpisodeTypeDto> GetEpisodeTypes() => _episodeTypeRepository.Value.GetAll()?
			.Select(e => new EpisodeTypeDto
			{
				EpisodeTypeId = e.EpisodeTypeId,
				EpisodeTypeName = e.TypeName,
				SortOrder = e.SortOrder
			}).OrderBy(x => x.EpisodeTypeName).ToList();

		public void ResolveEpisode(int episodeId, string modifiedByUserId)
		{
			var episodeEntity = _episodeRepository.Value.Get(episodeId);
			if (null == episodeEntity)
				throw new Exception($"Not not find Episode with ID {episodeId}");
			var now = DateTime.UtcNow;
			var user = _usersRepository.Value.Get(modifiedByUserId);
			episodeEntity.UpdatedOnUtc = now;
			episodeEntity.ModifiedByUser = user;
			episodeEntity.ResolvedDateUtc = now;
			_episodeRepository.Value.Save(episodeEntity);
		}

		public EpisodeBladeDto SaveNewEpisode(int claimId, byte? episodeTypeId, string pharmacyNabp, string rxNumber,
			string episodeText, string userId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("[dbo].[uspSaveNewEpisode]", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;	                
					var claimIdParam = cmd.CreateParameter();
					claimIdParam.Value = claimId;
					claimIdParam.ParameterName = "@ClaimID";
					claimIdParam.DbType = DbType.Int32;
					claimIdParam.SqlDbType = SqlDbType.Int;
					claimIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(claimIdParam);
					var episodeTypeIdParam = cmd.CreateParameter();
					episodeTypeIdParam.Value = episodeTypeId ?? (byte) 1;
					episodeTypeIdParam.ParameterName = "@EpisodeTypeID";
					episodeTypeIdParam.DbType = DbType.Byte;
					episodeTypeIdParam.SqlDbType = SqlDbType.TinyInt;
					episodeTypeIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(episodeTypeIdParam);
					var pharmacyNabpParam = cmd.CreateParameter();
					pharmacyNabpParam.Value = pharmacyNabp ?? (object) DBNull.Value;
					pharmacyNabpParam.ParameterName = "@PharmacyNABP";
					pharmacyNabpParam.DbType = DbType.AnsiString;
					pharmacyNabpParam.SqlDbType = SqlDbType.VarChar;
					pharmacyNabpParam.Size = 7;
					pharmacyNabpParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(pharmacyNabpParam);
					var rxNumberParam = cmd.CreateParameter();
					rxNumberParam.Value = rxNumber ?? (object)DBNull.Value;
					rxNumberParam.ParameterName = "@RxNumber";
					rxNumberParam.DbType = DbType.AnsiString;
					rxNumberParam.SqlDbType = SqlDbType.VarChar;
					rxNumberParam.Size = 100;
					rxNumberParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(rxNumberParam);
					var noteTextParam = cmd.CreateParameter();
					noteTextParam.Value = episodeText ?? (object)DBNull.Value;
					noteTextParam.ParameterName = "@NoteText";
					noteTextParam.DbType = DbType.AnsiString;
					noteTextParam.SqlDbType = SqlDbType.VarChar;
					noteTextParam.Size = 8000;
					noteTextParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(noteTextParam);
					var userIdParam = cmd.CreateParameter();
					userIdParam.Value = userId ?? (object)DBNull.Value;
					userIdParam.ParameterName = "@UserID";
					userIdParam.DbType = DbType.String;
					userIdParam.SqlDbType = SqlDbType.NVarChar;
					userIdParam.Size = 128;
					userIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(userIdParam);
					if (conn.State != ConnectionState.Open)
						conn.Open();
					return DisposableService.Using(cmd.ExecuteReader, reader =>
					{
						var idOrdinal = reader.GetOrdinal("Id");
						var createdOrdinal = reader.GetOrdinal("Created");
						var ownerOrdinal = reader.GetOrdinal("Owner");
						var typeOrdinal = reader.GetOrdinal("Type");
						var roleOrdinal = reader.GetOrdinal("Role");
						var pharmacyOrdinal = reader.GetOrdinal("Pharmacy");
						var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
						var resolvedOrdinal = reader.GetOrdinal("Resolved");
						var noteCountOrdinal = reader.GetOrdinal("NoteCount");
						var list = new List<EpisodeBladeDto>();
						while (reader.Read())
						{
							var episodeBladeDto = new EpisodeBladeDto
							{
								Id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : default (int),
								Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal) : (DateTime?) null,
								Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
								Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
								Role = !reader.IsDBNull(roleOrdinal) ? reader.GetString(roleOrdinal) : string.Empty,
								Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
								RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
								Resolved = !reader.IsDBNull(resolvedOrdinal) && reader.GetBoolean(resolvedOrdinal),
								NoteCount = !reader.IsDBNull(noteCountOrdinal) ? reader.GetInt32(noteCountOrdinal) : default (int)
							};
							list.Add(episodeBladeDto);
						}
						return list.SingleOrDefault();
					});
				});
			});

		public void AssignOrAcquireEpisode(int episodeId, string userId, string modifiedByUserId)
		{
			var episode = _episodeRepository.Value.GetSingleOrDefault(x => x.EpisodeId == episodeId);
			if (null == episode)
				throw new Exception($"Coult not find Episode Id {episodeId}");
			var user = _usersRepository.Value.GetSingleOrDefault(x => x.Id == userId);
			if (null == user)
				throw new Exception($"Error. Could not find a user with Id \"{userId}\"");
			var modifiedByUser = _usersRepository.Value.GetSingleOrDefault(x => x.Id == modifiedByUserId);
			if (null == modifiedByUser)
				throw new Exception($"Error, could not find a user with Id \"{modifiedByUserId}\"");
			if (null == episode.AcquiredUser)
				episode.AcquiredUser = user;
			episode.AssignedUser = user;
			episode.ModifiedByUser = modifiedByUser;
			episode.UpdatedOnUtc = DateTime.UtcNow;
			_episodeRepository.Value.Update(episode);
		}

		/// <summary>
		/// Saves a new Episode Note object to the databse.
		/// </summary>
		/// <param name="episodeId"></param>
		/// <param name="note"></param>
		/// <param name="userId"></param>
		/// <param name="today"></param>
		public void SaveEpisodeNote(int episodeId, string note, string userId, DateTime today)
		{
			var utcNow = DateTime.UtcNow;
			var enote = new EpisodeNote();
			var user = _usersRepository.Value.Get(userId);
			var episode = _episodeRepository.Value.Get(episodeId);
			enote.Created = today;
			enote.NoteText = note;
			enote.CreatedOnUtc = utcNow;
			enote.Episode = episode ?? throw new Exception($"Error, count not retrieve Episode Id {episodeId}");
			enote.UpdatedOnUtc = utcNow;
			enote.WrittenByUser = user ?? throw new Exception($"Error. Could not fine User Id \"{userId}\"");
			_episodeNoteRepository.Value.Save(enote);
		}
	}
}