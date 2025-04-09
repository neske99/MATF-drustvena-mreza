using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IntRole>
    {
        public void Configure(EntityTypeBuilder<IntRole> builder)
        {
            builder.HasData(
                new IntRole { 
                    Id=1,
                    Name= Roles.Buyer,
                    NormalizedName="BUYER"
                },
                new IntRole {
                    Id=2,
                    Name= Roles.Admin,
                    NormalizedName="ADMIN"
                });
        }
    }
}
