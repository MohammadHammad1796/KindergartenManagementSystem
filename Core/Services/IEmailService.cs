using KindergartenManagementSystem.Core.Helpers;
using System.Threading.Tasks;

namespace KindergartenManagementSystem.Core.Services
{
    public interface IEmailService
    {
        Task<Result> SendEmailAsync(string email, string subject, string message);
    }
}