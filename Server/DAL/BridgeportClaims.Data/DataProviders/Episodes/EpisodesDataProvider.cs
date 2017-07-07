using System;
using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
    
    public class EpisodesDataProvider : IEpisodesDataProvider
    {
        private readonly IRepository<Episode> _episodeRepository;

        public EpisodesDataProvider(IRepository<Episode> episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public async Task AddOrUpdateEpisode(EpisodeDto episode)
        {
            await Task.Run(() =>
            {
                // Insert
                if (null == episode.EpisodeId || 0 == episode.EpisodeId)
                {
                    var e = new Episode
                    {
                        CreatedDate = episode.Date,
                        AssignUser = episode.By,
                        Note = episode.Note
                    };
                    _episodeRepository.Save(e);
                }
                else
                {
                    var e = _episodeRepository.Get(episode.EpisodeId.Value);
                    if (null == e)
                        throw new ArgumentNullException(nameof(EpisodeDto));
                    e.CreatedDate = episode.Date;
                    e.AssignUser = episode.By;
                    e.Note = episode.Note;
                    _episodeRepository.Update(e);
                }
            });
        }
    }
}