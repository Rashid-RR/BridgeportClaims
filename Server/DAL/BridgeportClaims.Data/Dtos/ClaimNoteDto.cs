using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class ClaimNoteDto
    {
        public string NoteType { get; set; }
        public string NoteText { get; set; }
    }
}