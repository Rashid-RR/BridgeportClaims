using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class ClaimNoteDto
    {
        public KeyValuePair<int, string> NoteType { get; set; }
        public string NoteText { get; set; }
    }
}