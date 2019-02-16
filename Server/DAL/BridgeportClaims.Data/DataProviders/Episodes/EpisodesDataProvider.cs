using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;
using ic = BridgeportClaims.Common.Constants.IntegerConstants;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	
	public class EpisodesDataProvider : IEpisodesDataProvider
	{
		public void AssociateEpisodeToClaim(int episodeId, int claimId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspAssociateEpisodeToClaim]";
				conn.Open();
				conn.Execute(sp, new {EpisodeID = episodeId, ClaimID = claimId},
					commandType: CommandType.StoredProcedure);
			});

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
					documentIdParam.Value = default == documentId ? throw new Exception($"Error, document Id {documentId} is not a valid document.") : documentId;
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
			byte? episodeTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize, string userId, bool archived) =>
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
					pageNumberParam.Value = default == pageNumber ? ic.MaxRowCountForBladeInApp : pageNumber;
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
					var archivedParam = cmd.CreateParameter();
					archivedParam.Value = archived;
					archivedParam.Direction = ParameterDirection.Input;
					archivedParam.DbType = DbType.Boolean;
					archivedParam.SqlDbType = SqlDbType.Bit;
					archivedParam.ParameterName = "@Archived";
					cmd.Parameters.Add(archivedParam);
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
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
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
                                ClaimId = !reader.IsDBNull(claimIdOrdinal) ? reader.GetInt32(claimIdOrdinal) : default,
								Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
								Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
								Carrier = !reader.IsDBNull(carrierOrdinal) ? reader.GetString(carrierOrdinal) : string.Empty,
								EpisodeNoteCount = !reader.IsDBNull(episodeNoteCountOrdinal) ? reader.GetInt32(episodeNoteCountOrdinal) : default,
								FileUrl = !reader.IsDBNull(fileUrlOrdinal) ? reader.GetString(fileUrlOrdinal) : string.Empty
							};
							list.Add(result);
						}
					});
					if (conn.State != ConnectionState.Closed)
						conn.Close();
					retVal.EpisodeResults = list;
					retVal.TotalRowCount = totalPageSizeParam.Value as int? ?? default;
					return retVal;
				});
			});

		public IEnumerable<EpisodeTypeDto> GetEpisodeTypes() =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string query = "SELECT EpisodeTypeId = et.EpisodeTypeID, EpisodeTypeName = et.TypeName, et.SortOrder FROM dbo.EpisodeType AS et;";
				conn.Open();
				return conn.Query<EpisodeTypeDto>(query, commandType: CommandType.Text)
					?.OrderBy(x => x.EpisodeTypeName);
			});

		public void ResolveEpisode(int episodeId, string modifiedByUserId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspResolveEpisode]";
				conn.Open();
				conn.Execute(sp, new {EpisodeID = episodeId, ModifiedByUserID = modifiedByUserId},
					commandType: CommandType.StoredProcedure);
			});

		public EpisodeBladeDto SaveNewEpisode(int? claimId, byte? episodeTypeId, string pharmacyNabp, string rxNumber,
			string episodeText, string userId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("[dbo].[uspSaveNewEpisode]", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;	                
					var claimIdParam = cmd.CreateParameter();
					claimIdParam.Value = claimId ?? (object) DBNull.Value;
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
								Id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : default,
								Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal) : (DateTime?) null,
								Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
								Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
								Role = !reader.IsDBNull(roleOrdinal) ? reader.GetString(roleOrdinal) : string.Empty,
								Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
								RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
								Resolved = !reader.IsDBNull(resolvedOrdinal) && reader.GetBoolean(resolvedOrdinal),
								NoteCount = !reader.IsDBNull(noteCountOrdinal) ? reader.GetInt32(noteCountOrdinal) : default
							};
							list.Add(episodeBladeDto);
						}
						return list.SingleOrDefault();
					});
				});
			});

		public void AssignOrAcquireEpisode(int episodeId, string userId, string modifiedByUserId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspAssignOrAcquireEpisode]";
				conn.Open();
				conn.Execute(sp, new {EpisodeID = episodeId, UserID = userId, ModifiedByUserID = modifiedByUserId},
					commandType: CommandType.StoredProcedure);
			});

		/// <summary>
		/// Saves a new Episode Note object to the databse.
		/// </summary>
		/// <param name="episodeId"></param>
		/// <param name="note"></param>
		/// <param name="userId"></param>
		/// <param name="today"></param>
		public void SaveEpisodeNote(int episodeId, string note, string userId, DateTime today) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspEpisodeNoteInsert]";
				conn.Open();
				conn.Execute(sp, new {EpisodeID = episodeId, NoteText = note, UserID = userId, Today = today},
					commandType: CommandType.StoredProcedure);
			});


		public void ArchiveEpisode(int episodeId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				const string sp = "[dbo].[uspArchiveEpisode]";
				conn.Open();
				conn.Execute(sp, new {EpisodeID = episodeId}, commandType: CommandType.StoredProcedure);
			});
	}
}