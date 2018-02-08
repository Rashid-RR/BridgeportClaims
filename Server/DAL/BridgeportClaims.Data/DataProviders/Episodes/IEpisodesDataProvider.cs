using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	public interface IEpisodesDataProvider
	{
	    bool CreateImageCategoryEpisode(byte documentTypeId, int claimId, string rxNumber, string userId, int documentId);
        EpisodesDto GetEpisodes(DateTime? startDate, DateTime? endDate, bool resolved, string ownerId,
	        int? episodeCategoryId, byte? episodeTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
	    void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string noteText, byte? episodeTypeId);
        IList<EpisodeTypeDto> GetEpisodeTypes();
        void ResolveEpisode(int episodeId, string modifiedByUserId);
	    void SaveNewEpisode(int claimId, byte? episodeTypeId, string pharmacyNabp, string rxNumber, string episodeText, string userId);
        void AcquireEpisode(int episodeId, string userId);
        void SaveEpisodeNote(int episodeId, string note, string userId, DateTime today);
    }
}