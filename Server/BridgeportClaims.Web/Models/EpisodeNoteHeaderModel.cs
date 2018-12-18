using System;
using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public class EpisodeNoteHeaderModel
    {
        public int Id { get; set; }        
        public string Owner { get; set; }
        public DateTime? EpisodeCreated { get; set; }
        public string PatientName { get; set; }
        public string ClaimNumber { get; set; }
        public IList<EpisodeNoteModel> EpisodeNotes { get; set; }
    }
}