

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Post.Application;
public static class InfrastructureServiceRegistration
{
  public static void RegisterApplicationService(this IServiceCollection services/*, IConfiguration configuration*/)
  {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddMediatR(cfg=>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

  }

}
