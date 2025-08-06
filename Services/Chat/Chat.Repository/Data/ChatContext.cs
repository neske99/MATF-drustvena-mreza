using Microsoft.EntityFrameworkCore;
using Post.Infrastructure.Persistance.EntityConfigurations;
using Services.Chat.Chat.Model.Entities;
using Services.Chat.Chat.Model.Entities.Common;

namespace Chat.Repository.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext (DbContextOptions<ChatContext> options) : base(options)
        {

        }
        public DbSet<ChatGroup> ChatGroups { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ChatUser> ChatUsers { get; set; } = null!;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.CreatedById=entry.Entity.Id;
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = entry.Entity.Id;
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ChatGroupConfiguration());
            modelBuilder.ApplyConfiguration(new ChatUserConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }


}