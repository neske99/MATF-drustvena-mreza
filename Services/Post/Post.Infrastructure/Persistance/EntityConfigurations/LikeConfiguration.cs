using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Domain.Entities;

namespace Post.Infrastructure.Persistance.EntityConfigurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id).UseHiLo("postseqlike");

            builder.HasOne<Post.Domain.Entities.Post>().WithMany(p => p.Likes).HasForeignKey(l => l.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(l => l.User).WithMany().HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.NoAction);
            
            // Add unique constraint so a user can only like a post once
            builder.HasIndex(l => new { l.PostId, l.UserId }).IsUnique();
        }
    }
}