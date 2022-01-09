using KindergartenManagementSystem.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class InfrastructureDbContext : IdentityDbContext<InfrastructureUser, InfrastructureRole, int>
    {
        public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options)
            : base(options)
        {
        }

        public DbSet<AccessToken> AccessTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InfrastructureUser>()
                .HasMany(iu => iu.AccessTokens)
                .WithOne()
                .HasForeignKey(at => at.UserId);
        }
    }
}
