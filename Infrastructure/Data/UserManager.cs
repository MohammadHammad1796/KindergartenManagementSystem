using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Models;
using KindergartenManagementSystem.Core.Services;
using KindergartenManagementSystem.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<InfrastructureUser> _userManager;
        private InfrastructureUser _infrastructureUser;

        public UserManager(UserManager<InfrastructureUser> userManager)
        {
            _userManager = userManager;
            _infrastructureUser = new InfrastructureUser();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            _infrastructureUser = await _userManager.FindByEmailAsync(email);
            return _infrastructureUser.ToUser();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            user.ToInfrastructureUser(_infrastructureUser);
            return await _userManager.GeneratePasswordResetTokenAsync(_infrastructureUser);
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            user.ToInfrastructureUser(_infrastructureUser);
            return await _userManager.IsEmailConfirmedAsync(_infrastructureUser);
        }

        public async Task<Result> ResetPasswordAsync(User user, string token, string newPassword)
        {
            user.ToInfrastructureUser(_infrastructureUser);
            return (await _userManager.ResetPasswordAsync(_infrastructureUser, token, newPassword)).ToResult();
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            user.ToInfrastructureUser(_infrastructureUser);
            return await _userManager.GetRolesAsync(_infrastructureUser);
        }
    }
}
