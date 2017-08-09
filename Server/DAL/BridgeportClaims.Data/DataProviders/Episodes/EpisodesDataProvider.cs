using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using c = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	
	public class EpisodesDataProvider : IEpisodesDataProvider
	{
		private readonly ISessionFactory _sessionFactory;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IRepository<EpisodeType> _episodeTypeRepository;

		public EpisodesDataProvider(ISessionFactory sessionFactory, IRepository<EpisodeType> episodeTypeRepository)
		{
			_sessionFactory = sessionFactory;
			_episodeTypeRepository = episodeTypeRepository;
		}

		public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId)
		{
			DisposableService.Using(() => new SqlConnection(c.GetDbConnStr()), conn =>
			{
				DisposableService.Using(() => new SqlCommand("uspSaveEpisode", conn), cmd =>
				{
					conn.Open();
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
					cmd.ExecuteNonQuery();
				});
			});
		}


		/*public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId)
				=> DisposableService.Using(() => _sessionFactory.OpenSession(),
					session =>
					{
						DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
							transaction =>
							{
								try
								{   // Upsert
									session.CreateSQLQuery("EXECUTE [dbo].[uspSaveEpisode] " +
														   "@EpisodeID = :EpisodeID, @ClaimID = :ClaimID, " +
														   "@CreatedDateUTC = :CreatedDateUTC," +
														   "@AssignedUserID = :AssignedUserID, @Note = :Note," +
														   "@EpisodeTypeID = :EpisodeTypeID")
										.SetInt32("EpisodeID", episodeId)
										.SetInt32("ClaimID", claimId)
										.SetDateTime2("CreatedDateUTC", DateTime.UtcNow)
										.SetString("AssignedUserID", by)
										.SetString("Note", noteText)
										.SetInt32("EpisodeTypeID", episodeTypeId)
										.SetMaxResults(1)
										.ExecuteUpdate();
									if (transaction.IsActive)
										transaction.Commit();
								}
								catch (Exception ex)
								{
									Logger.Error(ex);
									if (transaction.IsActive)
										transaction.Rollback();
									throw;
								}
							});
					});*/

		public IList<EpisodeTypeDto> GetEpisodeTypes()
		{
			var retVal = _episodeTypeRepository.GetAll()
				.Select(e => new EpisodeTypeDto
				{
					EpisodeTypeId = e.EpisodeTypeId,
					EpisodeTypeName = e.TypeName
				}).ToList();
			return retVal;
		}
	}
}