using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindergartenManagementSystem.Core.Services
{
    public interface IUserManager
    {
        Task<User> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<Result> ResetPasswordAsync(User user, string token, string newPassword);
        Task<IList<string>> GetRolesAsync(User user);
    }
}
