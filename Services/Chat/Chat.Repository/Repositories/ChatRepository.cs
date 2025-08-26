using Chat.Model.DTOs;
using Chat.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Services.Chat.Chat.Model.Entities;

namespace Chat.Repository.Repositories.Contracts

{
  public class ChatRepository : IChatRepository
  {
    private readonly ChatContext _chatContext;
    public ChatRepository(ChatContext chatContext)
    {
      _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
    }

    public async Task<List<User>> GetAllUsers()
    {
      return await _chatContext.Users
        .ToListAsync();
    }

    public async Task<List<ChatGroupDTO>> GetChatGroupForUserAsync(int userId)
    {
      return await _chatContext.ChatGroups
        .Include(cg => cg.ChatUsers)
        .ThenInclude(cu => cu.User)
        .Where(cg => cg.ChatUsers.Any(cu => cu.UserId == userId))
        .Select(cg=> new ChatGroupDTO
        {
          ChatId = cg.Id,
          UserId = cg.ChatUsers.FirstOrDefault(cu => cu.UserId == userId).UserId,
          Username = cg.ChatUsers.FirstOrDefault(cu => cu.UserId != userId).User.Username ?? string.Empty,
          HasNewMessages = cg.ChatUsers.FirstOrDefault(cu => cu.UserId == userId).hasNewMessages

        }).OrderBy(cg=> cg.HasNewMessages==true ?0 :1)
        .ToListAsync();
    }

    public async Task<List<ChatMessageDTO>> GetMessagesForGroupAsync(int userId, int chatGroupId)
    {
      return await _chatContext.ChatMessages
        //.Include(cm => cm.UserId)
        .Where(cm => cm.ChatGroupId == chatGroupId)
        .OrderBy(cm => cm.CreatedDate)
        .Take(100)
        .Select(cm => new ChatMessageDTO
        {
          Id=cm.Id,
          IsSender = cm.UserId == userId,
          Message = cm.Text,
          Timestamp = cm.CreatedDate 
        })
        .ToListAsync();
    }

    public async Task<List<ChatUser>> GetUsersForGroupAsync(int userId, int chatGroupId)
    {
      return await _chatContext.ChatUsers
        .Include(cu => cu.User)
        .Where(cu => cu.ChatGroupId == chatGroupId && cu.UserId != userId)
        .ToListAsync();
    }

    public async Task<bool> ReplicateUser(User userToReplicate)
    {
      string toExecute = "SET IDENTITY_INSERT [dbo].[User] ON " +
            $"INSERT INTO [dbo].[User] (Id,Username,CreatedDate) VALUES ({userToReplicate.Id},'{userToReplicate.Username}','{userToReplicate.CreatedDate}')" +
            " SET IDENTITY_INSERT [dbo].[User] OFF";
      var result = await _chatContext.Database.ExecuteSqlRawAsync(toExecute);

      return result > 0;
    }

    public async Task<ChatMessage> CreateMessageForChatGroupAsync(int userId, int chatGroupId, string message)
    {
      var newMessage = new ChatMessage
      {
        UserId = userId,
        ChatGroupId = chatGroupId,
        CreatedDate = DateTime.UtcNow,
        Text = message
      };
      _chatContext.ChatMessages.Add(newMessage);
      var users=_chatContext.ChatUsers.Where(cu => cu.ChatGroupId == chatGroupId ).ToList();

      foreach (var user in users)
      {
        if(user.UserId != userId)
        {
          user.hasNewMessages = true;
        }else
          user.hasNewMessages = false;
      }

      var result = await _chatContext.SaveChangesAsync();
      return newMessage;
    }

    public async Task<ChatGroup> CreateChatGroupAsync(int userAId, int userBId)
    {
      var chatGroup = new ChatGroup
      {
        CreatedDate = DateTime.UtcNow,
        ChatUsers = new List<ChatUser>
        {
          new ChatUser { UserId = userAId },
          new ChatUser { UserId = userBId }
        }
      };

      _chatContext.ChatGroups.Add(chatGroup);
      var result = await _chatContext.SaveChangesAsync();

      return await _chatContext.ChatGroups
        .Include(cg => cg.ChatUsers)
        .ThenInclude(cu => cu.User).
        FirstOrDefaultAsync(cg => cg.Id==chatGroup.Id );
;

    }

    public async Task SetChatUsersHasNewMessagesAsync(int chatGroupId, int userId)
    {

      await _chatContext.ChatUsers
        .Where(cu => cu.ChatGroupId == chatGroupId && cu.UserId == userId)
        .ExecuteUpdateAsync(cu => cu.SetProperty(c => c.hasNewMessages, false));

      await _chatContext.ChatUsers
        .Where(cu => cu.ChatGroupId == chatGroupId && cu.UserId != userId)
        .ExecuteUpdateAsync(cu => cu.SetProperty(c => c.hasNewMessages, true));

    }
  }
}