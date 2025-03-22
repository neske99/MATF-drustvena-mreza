using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistance.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(x=>x.Id);
            builder.Property(c=>c.Id).UseHiLo("postseqcomment");
            
            builder.HasOne<Post.Domain.Entities.Post>().WithMany(p=>p.Comments).HasForeignKey(c=>c.PostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}