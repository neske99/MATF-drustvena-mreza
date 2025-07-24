using Microsoft.Extensions.DependencyInjection;
using Relations.Common.Data;
using Relations.Common.Repositories;

namespace Relations.Common.Extensions
{
    public static class RelationsCommonExtensions
    {
        public static void AddRelationsCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IRelationsRepository, RelationsRepository>();
            services.AddScoped<IRelationsContext, RelationsContext>();
        }
    }
}
