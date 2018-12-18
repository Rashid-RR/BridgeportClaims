using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public class CarriersModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<int> PayorIds { get; set; }
        public bool Archived { get; set; } = false;
    }
}