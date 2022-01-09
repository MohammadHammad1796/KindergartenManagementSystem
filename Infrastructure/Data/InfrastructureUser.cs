using KindergartenManagementSystem.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class InfrastructureUser : IdentityUser<int>
    {
        public ICollection<AccessToken> AccessTokens { get; set; }
    }
}