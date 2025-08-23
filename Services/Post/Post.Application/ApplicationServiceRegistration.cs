

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Post.Application;
public static class InfrastructureServiceRegistration
{
  public static IServiceCollection RegisterApplicationService(this IServiceCollection services/*, IConfiguration configuration*/)
  {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddMediatR(cfg=>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    return services;
  }

}
