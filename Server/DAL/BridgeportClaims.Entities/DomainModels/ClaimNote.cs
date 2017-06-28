﻿using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

namespace BridgeportClaims.Entities.DomainModels
{
    public class ClaimNote
    {
        [Required]
        public virtual int ClaimNoteId { get; set; }
        public virtual Claim Claim { get; set; }
        public virtual ClaimNoteType ClaimNoteType { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [Required]
        [StringLength(8000)]
        public virtual string NoteText { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
    }
}
