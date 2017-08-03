using System;

namespace BridgeportClaims.Data.Dtos
{
	[Serializable]
	public sealed class ImportFileDto
	{
		public int ImportFileId { get; set; }
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public string FileDescription { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
