using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public sealed class CollectionAssignmentModel
    {
        public string UserId { get; set; }
        public IList<int> PayorIds { get; set; }
    }
}