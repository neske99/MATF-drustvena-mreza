using Chat.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
  private readonly IChatService _chatService;
  public ChatController(IChatService chatService)
  {
    _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
  }

  [HttpGet(Name = "Ping")]
  public async Task<ActionResult> Test()
  {
    return Ok("Pong");
  }


  [HttpGet("ChatGroupForGroup")]
  public async Task<ActionResult> GetUserForGroupAsync([FromQuery] int userId, [FromQuery] int chatGroupId)
  {
    return Ok(await _chatService.GetChatGroupForGroupAsync(userId));
  }

  [HttpGet("MessagesByChatGroup")]
  public async Task<ActionResult> MessagesByChatGroup([FromQuery] int userId, [FromQuery] int chatGroupId)
  {
    return Ok(await _chatService.GetMessagesForGroupAsync(userId, chatGroupId));
  }


  [HttpGet("UsersForGroup")]
  public async Task<ActionResult> GetUsersForGroup([FromQuery] int userId, [FromQuery] int chatGroupId)
  {
    return Ok(await _chatService.GetUsersForGroupAsync(userId, chatGroupId));
  }

  [HttpGet("GetAllUsers")]
  public async Task<ActionResult> GetUsers()
  {
    return Ok(await _chatService.GetAllUsers());
  }


}