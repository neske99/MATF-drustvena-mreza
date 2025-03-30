using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { 
                    Name= Roles.Buyer,
                    NormalizedName="BUYER"
                },
                new IdentityRole {
                    Name= Roles.Admin,
                    NormalizedName="ADMIN"
                });
        }
    }
}
