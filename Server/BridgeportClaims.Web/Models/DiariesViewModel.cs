using System;

namespace BridgeportClaims.Web.Models
{
    public class DiariesViewModel
    {
        public bool IsDefaultSort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}