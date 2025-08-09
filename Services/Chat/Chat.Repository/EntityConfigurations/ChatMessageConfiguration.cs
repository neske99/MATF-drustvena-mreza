using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Chat.Chat.Model.Entities;

namespace Post.Infrastructure.Persistance.EntityConfigurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {

            builder.ToTable("ChatMessage");
            builder.HasKey(cm => cm.Id);
            builder.Property(cm => cm.Id).UseHiLo("postseqcomment");


            builder.HasOne<ChatGroup>().WithMany().HasForeignKey(cm => cm.ChatGroupId);
            builder.HasOne<User>().WithMany().HasForeignKey(cm => cm.UserId );

            /*
            builder.ToTable("Comments");
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id).UseHiLo("postseqcomment");

            builder.HasOne<Post.Domain.Entities.Post>().WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c=>c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
            */
        }
    }
}