using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chat.Repository.Data
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<ChatContext>
    {
        public ChatContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ChatContext>();
            //builder.UseSqlServer("Server=localhost,8091;Database=PostDb;User Id=sa;Password=MATF12345678rs2;TrustServerCertificate=True;Encrypt=False;");
            return new ChatContext(builder.Options);
        }
    }
}