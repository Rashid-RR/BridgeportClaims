namespace BridgeportClaims.Web.Models
{
    public class SuspenseViewModel
    {
        public string SessionId { get; set; }
        public int DocumentId { get; set; }
        public decimal AmountToSuspense { get; set; }
        public string NoteText { get; set; }
        // The following fields will only used if a Posting session object is not already in memory.
        public string CheckNumber { get; set; }
    }
}