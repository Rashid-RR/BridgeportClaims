using System;

namespace BridgeportClaims.Web.Models
{
    public class EpisodeNoteModel
    {
        public string WrittenBy { get; set; }
        public DateTime NoteCreated { get; set; }
        public string NoteText { get; set; }
    }
}