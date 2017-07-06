using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimNoteDto
    {
        public string NoteType { get; set; }
        public string NoteText { get; set; }
    }
}