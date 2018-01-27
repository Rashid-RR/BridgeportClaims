using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	public interface IEpisodesDataProvider
	{
	    EpisodesDto GetEpisodes(bool resolved, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId);
		IList<EpisodeTypeDto> GetEpisodeTypes();
	}
}