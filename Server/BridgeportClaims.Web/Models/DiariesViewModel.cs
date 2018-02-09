using System;

namespace BridgeportClaims.Web.Models
{
    public class DiariesViewModel
    {
        public bool IsDefaultSort { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool Closed { get; set; }
    }
}