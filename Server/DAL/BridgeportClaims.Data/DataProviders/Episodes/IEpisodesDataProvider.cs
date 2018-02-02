using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	public interface IEpisodesDataProvider
	{
	    void CreateImageCategoryEpisode(int claimId, string userId, string fileNameNote,
	        DateTime created, int documentId, int documentTypeId, string rxNumber = null);
        EpisodesDto GetEpisodes(DateTime? startDate, DateTime? endDate, bool resolved, string ownerId,
	        int? episodeCategoryId, int? episodeTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, int? episodeTypeId);
		IList<EpisodeTypeDto> GetEpisodeTypes();
        void ResolveEpisode(int episodeId, string modifiedByUserId);
	    void SaveNewEpisode(int claimId, int? episodeTypeId, string pharmacyNabp, string rxNumber, string episodeText, string userId);

	}
}