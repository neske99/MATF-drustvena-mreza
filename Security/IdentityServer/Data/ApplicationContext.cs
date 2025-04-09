using IdentityServer.Entities;
using IdentityService.Data.EntityConfigurations;
using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace IdentityService.Data
{
    public class ApplicationContext:IdentityDbContext<User,IntRole,int>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ApplicationContext( DbContextOptions options) :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
