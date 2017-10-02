using System;
using System.ComponentModel.DataAnnotations;

namespace LakerFileImporter.DAL.Dtos
{
	[Serializable]
	public sealed class ImportFileDto
	{
		[Required]
		public int ImportFileId { get; set; }
		[Required]
		[StringLength(255)]
		public string FileName { get; set; }
		[StringLength(30)]
		public string FileExtension { get; set; }
		[Required]
		[StringLength(100)]
		public string FileSize { get; set; }
		[Required]
		public string FileType { get; set; }
		[Required]
		public bool Processed { get; set; }
		[Required]
		public DateTime? CreatedOn { get; set; }
	}
}
