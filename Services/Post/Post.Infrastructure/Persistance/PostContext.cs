using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Domain.Common;
using Post.Domain.Entities;
using Post.Infrastructure.Persistance.EntityConfigurations;

namespace Post.Infrastructure.Persistance
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options) : base(options)
        {

        }
        public DbSet<Post.Domain.Entities.Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

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
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }


}