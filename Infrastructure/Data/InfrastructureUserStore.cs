using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class InfrastructureUserStore : UserStore<InfrastructureUser, InfrastructureRole, InfrastructureDbContext, int>
    {
        public InfrastructureUserStore(InfrastructureDbContext context) : base(context)
        {
            AutoSaveChanges = false;
        }
    }
}