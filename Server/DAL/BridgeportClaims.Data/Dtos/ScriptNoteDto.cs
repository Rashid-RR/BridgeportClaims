﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ScriptNoteDto
    {
        public int ClaimId { get; set; }
        public int PrescriptionNoteId { get; set; }
        public IList<ScriptDto> Scripts { get; set; }
        public string Type { get; set; }
        public string EnteredBy { get; set; }
        public string Note { get; set; }
        public DateTime? NoteUpdatedOn { get; set; }
        public bool HasDiaryEntry { get; set; }
    }
}