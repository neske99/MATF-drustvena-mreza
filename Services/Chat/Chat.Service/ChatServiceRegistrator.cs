using Chat.Repository;
using Chat.Service.Services;
using Chat.Service.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Service;

public static class ChatServiceRegistrator
{
  public static void RegisterChatServices(this IServiceCollection services,IConfiguration configuration)
  {
    // Register the repository service
    services.RegisterRepositoryService(configuration);

    // Register the chat service
    services.AddScoped<IChatService, ChatService>();
  }

}



