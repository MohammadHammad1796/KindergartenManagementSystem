using System.Threading.Tasks;

namespace KindergartenManagementSystem.Core.Services
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}