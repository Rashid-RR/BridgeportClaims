using System;
using System.ComponentModel.DataAnnotations;

namespace BridgeportClaims.Entities.DomainModels
{
    public class Diary
    {
        [Required]
        public virtual int DiaryId { get; set; }
        public virtual DiaryType DiaryType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Claim Claim { get; set; }
        [Required]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
    }
}