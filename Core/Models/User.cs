using System;
using System.Collections.Generic;

namespace KindergartenManagementSystem.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public int AccessFailedCount { get; set; }

        public ICollection<AccessToken> AccessTokens { get; set; }
    }
}
