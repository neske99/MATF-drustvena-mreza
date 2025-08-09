
using Chat.Repository.Data;
using Chat.Repository.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Repository
{
    public static class RepositoryServiceRegistration
    {
        public static void RegisterRepositoryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddDbContext<ChatContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("chatdb"));
                //options.UseSqlServer("Server=localhost,8096;Database=ChatDb;User Id=sa;Password=MATF12345678rs2;TrustServerCertificate=True;");
            });
        }

    }
}