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
using Post.Application;
using Post.Infrastructure.Contracts;
using Post.Infrastructure.GrpcServices;
using Relations.GRPC;

namespace Post.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection RegisterInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterApplicationService();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddGrpcClient<RelationsProtoService.RelationsProtoServiceClient>(o =>
            {
                o.Address = new Uri(configuration.GetValue<string>("GrpcSettings:RelationsUrl"));
            });

            services.AddScoped<IRelationService, RelationsService>();
            services.AddDbContext<PostContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("mssql"));
                //options.UseSqlServer("Server=localhost,8091;Database=PostDb;User Id=sa;Password=MATF12345678rs2;TrustServerCertificate=True;");
            });
            return services;
        }

    }
}