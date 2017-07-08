using System.Data;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using NHibernate;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
    
    public class EpisodesDataProvider : IEpisodesDataProvider
    {
        private readonly ISessionFactory _sessionFactory;

        public EpisodesDataProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void AddOrUpdateEpisode(EpisodeDto episode)
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
                                                           "@CreatedDate = :CreatedDate," +
                                                           " @AssignUser = :AssignUser, @Note = :Note")
                                        .SetInt32("EpisodeID", episode.EpisodeId)
                                        .SetInt32("ClaimID", episode.ClaimId)
                                        .SetDateTime2("CreatedDate", episode.Date)
                                        .SetString("AssignUser", episode.By)
                                        .SetString("Note", episode.Note)
                                        .SetMaxResults(1)
                                        .ExecuteUpdate();
                                    if (transaction.IsActive)
                                        transaction.Commit();
                                }
                                catch
                                {
                                    if (transaction.IsActive)
                                        transaction.Rollback();
                                    throw;
                                }
                            });
                    });
    }
}