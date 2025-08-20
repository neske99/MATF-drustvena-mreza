using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Logger.Extensions
{
    public static class LoggerExtensions
    {
        /// Adds the logger middleware services with default configuration.
        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            services.Configure<LoggerOptions>(options => { });
            services.AddSingleton<IFileLogger>(provider =>
            {
                var options = provider.GetService<IOptions<LoggerOptions>>()?.Value ?? new LoggerOptions();
                return new FileLogger(options.LogDirectory);
            });
            return services;
        }

        /// Adds the logger middleware services with configuration provided via Action delegate.
        public static IServiceCollection AddLogger(this IServiceCollection services, Action<LoggerOptions> configureOptions)
        {
            services.Configure<LoggerOptions>(configureOptions);

            services.AddSingleton<IFileLogger>(provider =>
            {
                var loggerOptions = provider.GetService<IOptions<LoggerOptions>>()?.Value ?? new LoggerOptions();
                return new FileLogger(loggerOptions.LogDirectory);
            });

            return services;
        }

        /// Adds the logger middleware to the application pipeline.
        public static IApplicationBuilder UseLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggerMiddleware>();
        }
    }
}