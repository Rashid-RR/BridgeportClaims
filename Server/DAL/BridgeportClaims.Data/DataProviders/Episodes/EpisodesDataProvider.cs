using System;
using System.Data;
using BridgeportClaims.Common.Disposable;
using NHibernate;
using BridgeportClaims.Common.Extensions;
using NLog;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
    
    public class EpisodesDataProvider : IEpisodesDataProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public EpisodesDataProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string note)
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
                                                           " @AssignUser = :AssignUser, @Note = :Note")
                                        .SetInt32("EpisodeID", episodeId)
                                        .SetInt32("ClaimID", claimId)
                                        .SetDateTime2("CreatedDateUTC", DateTime.UtcNow)
                                        .SetString("AssignUser", by)
                                        .SetString("Note", note)
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
    }
}