using System.Linq;
using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Models;
using KindergartenManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace KindergartenManagementSystem.Extensions
{
    public static class MappingModelsWithIdentityExtensions
    {
        public static User ToUser(this InfrastructureUser infrastructureUser)
        {
            if (infrastructureUser == null)
                return null;

            return new User
            {
                Id = infrastructureUser.Id,
                AccessFailedCount = infrastructureUser.AccessFailedCount,
                ConcurrencyStamp = infrastructureUser.ConcurrencyStamp,
                Email = infrastructureUser.Email,
                EmailConfirmed = infrastructureUser.EmailConfirmed,
                LockoutEnabled = infrastructureUser.LockoutEnabled,
                LockoutEnd = infrastructureUser.LockoutEnd,
                NormalizedEmail = infrastructureUser.NormalizedEmail,
                NormalizedUserName = infrastructureUser.NormalizedUserName,
                PasswordHash = infrastructureUser.PasswordHash,
                UserName = infrastructureUser.UserName,
                SecurityStamp = infrastructureUser.SecurityStamp
            };
        }

        public static InfrastructureUser ToInfrastructureUser(this User user, InfrastructureUser infrastructureUser)
        {

            infrastructureUser.Id = user.Id;
            infrastructureUser.AccessFailedCount = user.AccessFailedCount;
            infrastructureUser.ConcurrencyStamp = user.ConcurrencyStamp;
            infrastructureUser.Email = user.Email;
            infrastructureUser.EmailConfirmed = user.EmailConfirmed;
            infrastructureUser.LockoutEnabled = user.LockoutEnabled;
            infrastructureUser.LockoutEnd = user.LockoutEnd;
            infrastructureUser.NormalizedEmail = user.NormalizedEmail;
            infrastructureUser.NormalizedUserName = user.NormalizedUserName;
            infrastructureUser.PasswordHash = user.PasswordHash;
            infrastructureUser.UserName = user.UserName;
            infrastructureUser.SecurityStamp = user.SecurityStamp;

            return infrastructureUser;
        }

        public static Result ToResult(this IdentityResult identityResult)
        {
            return new Result
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(e => new Error
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToList()
            };
        }

        public static Role ToRole(this InfrastructureRole infrastructureRole)
        {
            if (infrastructureRole == null)
                return null;

            return new Role
            {
                Id = infrastructureRole.Id,
                ConcurrencyStamp = infrastructureRole.ConcurrencyStamp,
                Name = infrastructureRole.Name,
                NormalizedName = infrastructureRole.NormalizedName
            };
        }

        public static InfrastructureRole ToInfrastructureRole(this Role role, InfrastructureRole infrastructureRole)
        {

            infrastructureRole.Id = role.Id;
            infrastructureRole.ConcurrencyStamp = role.ConcurrencyStamp;
            infrastructureRole.Name = role.Name;
            infrastructureRole.NormalizedName = role.NormalizedName;

            return infrastructureRole;
        }
    }
}
