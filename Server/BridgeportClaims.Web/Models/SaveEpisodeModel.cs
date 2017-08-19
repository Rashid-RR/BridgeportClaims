using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Web.Models
{
	public sealed class SaveEpisodeModel
	{
		public int? EpisodeId { get; set; }
		[Required]
		public int ClaimId { get; set; }
		[Required]
		[MinLength(1)]
		public string NoteText { get; set; }
		public int? EpisodeTypeId { get; set; }
	}
}