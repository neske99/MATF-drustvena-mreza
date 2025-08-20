using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Logger.Extensions
{
    public static class LoggerExtensions
    {
        /// Adds the logger middleware services to the dependency injection container.
        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            services.Configure<LoggerOptions>(options => { });
            return services;
        }

        /// Adds the logger middleware services with custom options.
        public static IServiceCollection AddLogger(this IServiceCollection services, Action<LoggerOptions> configureOptions)
        {
            services.Configure<LoggerOptions>(configureOptions);
            return services;
        }

        /// Adds the logger middleware to the application pipeline.
        public static IApplicationBuilder UseLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggerMiddleware>();
        }
    }
}