using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using NLog;

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

		public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string note, int? episodeTypeId)
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
														   "@AssignUser = :AssignUser, @Note = :Note," +
														   "@EpisodeTypeID = :EpisodeTypeID")
										.SetInt32("EpisodeID", episodeId)
										.SetInt32("ClaimID", claimId)
										.SetDateTime2("CreatedDateUTC", DateTime.UtcNow)
										.SetString("AssignUser", by)
										.SetString("Note", note)
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
					});

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