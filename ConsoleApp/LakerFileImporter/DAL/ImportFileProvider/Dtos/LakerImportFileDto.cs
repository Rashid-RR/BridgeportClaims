using System;
using System.ComponentModel.DataAnnotations;
using LakerFileImporter.Helpers;

namespace LakerFileImporter.DAL.ImportFileProvider.Dtos
{
	internal sealed class LakerImportFileDto
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

		public DateTime FileDate => ImportFilesDateParsingHelper.GetDateTimeParsedFromFileName(FileName);
	}
}
