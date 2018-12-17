namespace BridgeportClaims.Web.Framework.Models
{
    public sealed class SaveClaimNoteModel
    {
        public int ClaimId { get; set; }
        public string NoteText { get; set; }
        public int? NoteTypeId { get; set; }
    }
}