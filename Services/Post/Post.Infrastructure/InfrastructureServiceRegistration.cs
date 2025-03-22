using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Post.Application.Contracts;
using Post.Infrastructure.Persistance;
using Post.Infrastructure.Repositories;

namespace Post.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static  void RegisterInfrastructureService(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddScoped<IPostRepository,PostRepository>();
            services.AddDbContext<PostContext>(options =>{
                //options.UseSqlServer(configuration.GetConnectionString("mssql"));
                 options.UseSqlServer("Server=localhost;Database=PostDb;User Id=sa;Password=MATF12345678rs2;TrustServerCertificate=True;Encrypt=False;");
            });
        }
        
    }
}