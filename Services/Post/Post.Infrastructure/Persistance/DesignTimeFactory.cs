using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Post.Infrastructure.Persistance
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<PostContext>
    {
        public PostContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PostContext>();
            builder.UseSqlServer("Server=localhost;Database=PostDb;User Id=sa;Password=MATF12345678rs2;TrustServerCertificate=True;Encrypt=False;");
            return new PostContext(builder.Options);
        }
    }
}