using Microsoft.AspNetCore.SignalR;

namespace Chat.Chat.API
{
  public class ChatHub : Hub
  {

    public async Task SendMessage(string user, string message)
    {
      await Clients.Caller.SendAsync("ReceiveMessage", user, message);

    }
  }
}