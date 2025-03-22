using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistance.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post.Domain.Entities.Post>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Post> builder)
        {
            builder.ToTable("Posts");
            builder.Property(p => p.Id).UseHiLo("postseq");
            
            builder.HasOne<User>().WithMany().HasForeignKey(p=>p.UserId);
        }
    }
}