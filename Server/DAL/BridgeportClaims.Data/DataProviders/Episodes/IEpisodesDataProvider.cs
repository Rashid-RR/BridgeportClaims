using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	public interface IEpisodesDataProvider
	{
	    EpisodesDto GetEpisodes(bool resolved, string ownerId, int? episodeCategoryId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId);
		IList<EpisodeTypeDto> GetEpisodeTypes();
        void ResolveEpisode(int episodeId, string modifiedByUserId);
	    void SaveNewEpisode(int claimId, int? episodeTypeId, string pharmacyNabp, string rxNumber, string episodeText, string userId);

	}
}