using Chat.Service.Services;
using Chat.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Chat.API
{
  //[Authorize]
  public class ChatHub : Hub
  {
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
      _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
    }

    public override async Task OnConnectedAsync()
    {
      var usr = Context.User;

      if (usr?.Identity?.IsAuthenticated == true)
      {
        var userId = int.Parse(usr.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        var chatGroups = await _chatService.GetChatGroupForGroupAsync(userId);
        for (int i = 0; i < chatGroups.Count; i++)
          await Groups.AddToGroupAsync(Context.ConnectionId, chatGroups[i].ChatId.ToString());

    }


      await base.OnConnectedAsync();
    }

    public async Task RegisterToGroup(int chatGroupId)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId.ToString());
    }
    /*public async Task SendMessage(string user, string message)
    {
      var usr = Context.User;

      if (usr?.Identity?.IsAuthenticated == true)
      {
        // Example: get user ID from the claims (usually ClaimTypes.NameIdentifier or "sub")
        var userId = usr.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


        await Clients.Caller.SendAsync("ReceiveMessage", user, $"Welcome user {userId}!");
      }
      else
      {
        // User is not authenticated (should not happen if [Authorize] is present)
        await Clients.Caller.SendAsync("ReceiveMessage", "You are not authenticated.");
      }

    }*/


    public async Task SendMessageToGroup(int chatGroupId, string message)
    {
      var usr = Context.User;

      if (usr?.Identity?.IsAuthenticated == true)
      {
        // Example: get user ID from the claims (usually ClaimTypes.NameIdentifier or "sub")
        var userId = int.Parse(usr.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        // Or if your token uses "sub" claim for user ID:
        // var userId = user.FindFirst("sub")?.Value;


        //TODO: Add message to chat group in the database
        var res=await _chatService.CreateMessageForChatGroupAsync(userId, chatGroupId, message);

        await Clients.Group(chatGroupId.ToString()).SendAsync("ReceiveMessageReal", userId,message,chatGroupId,res.Id);

        //await Clients.Group(chatGroupId.ToString()).SendAsync("ReceiveMessageReal", userId,message,chatGroupId);
        //await Clients.Group(chatGroup).SendAsync("ReceiveMessageReal", userId,chatGroup,message);
      }


    }
  }
}