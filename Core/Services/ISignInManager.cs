using KindergartenManagementSystem.Core.Models;
using System.Threading.Tasks;
using KindergartenManagementSystem.Core.Helpers;

namespace KindergartenManagementSystem.Core.Services
{
    public interface ISignInManager
    {
        Task<Jwt> GenerateAuthenticationTokenAsync(User user);
        bool IsSignInRequireConfirmedEmail();
        Task<bool> CheckPasswordAsync(User user, string password);
        Task InvalidateToken(string token, User user, bool invalidateAll);
        Task RemoveExpiredTokensAsync();
    }
}