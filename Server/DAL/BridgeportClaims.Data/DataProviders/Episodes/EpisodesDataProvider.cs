using System;
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
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	
	public class EpisodesDataProvider : IEpisodesDataProvider
	{
		private readonly IRepository<EpisodeType> _episodeTypeRepository;
	    private readonly IMemoryCacher _memoryCacher;

		public EpisodesDataProvider(IRepository<EpisodeType> episodeTypeRepository)
		{
		    _episodeTypeRepository = episodeTypeRepository;
		    _memoryCacher = MemoryCacher.Instance;
		}

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