using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Episodes
{
	public interface IEpisodesDataProvider
	{
		void AddOrUpdateEpisode(int? episodeId, int claimId, string by, string note, int? episodeTypeId);
		IList<EpisodeTypeDto> GetEpisodeTypes();
	}
}