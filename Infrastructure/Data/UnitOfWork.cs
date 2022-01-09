using System.Threading.Tasks;
using KindergartenManagementSystem.Core.Services;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InfrastructureDbContext _dbContext;

        public UnitOfWork(InfrastructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}