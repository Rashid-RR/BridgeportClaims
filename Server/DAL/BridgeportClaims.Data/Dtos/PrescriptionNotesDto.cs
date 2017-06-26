﻿using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public class PrescriptionNotesDto
    {
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string EnteredBy { get; set; }
        public string Note { get; set; }
        public IList<KeyValuePair<int, string>> PrescriptionNoteTypes { get; set; }
    }
}
