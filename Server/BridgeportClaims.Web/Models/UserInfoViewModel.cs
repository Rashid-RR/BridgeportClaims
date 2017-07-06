using System;
using System.Collections.Generic;

namespace BridgeportClaims.Web.Models
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool HasRegistered { get; set; }
        public bool EmailConfirmed { get; set; }
        public string LoginProvider { get; set; }
        // Add our custom properties from ApplicationUser:
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisteredDate { get; set; }
        public IList<string> Roles { get; set; }
    }
}