﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BridgeportClaims.Web.Models
{
    public sealed class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool Deactivated { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Extension { get; set; }
        public byte? ReferralTypeId { get; set; }
        public IList<string> Roles { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}