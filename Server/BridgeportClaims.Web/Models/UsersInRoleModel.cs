using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public sealed class                                                                                                                                                                                                                         UsersInRoleModel
    {
        public string Id { get; set; }
        public IList<string> EnrolledUsers { get; set; }
        public IList<string> RemovedUsers { get; set; }
    }
}