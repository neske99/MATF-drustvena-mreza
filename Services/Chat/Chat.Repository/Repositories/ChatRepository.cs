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

    public async Task<List<ChatGroup>> GetChatGroupForUserAsync(int userId)
    {
      return await _chatContext.ChatGroups
        .Include(cg => cg.ChatUsers)
        .ThenInclude(cu => cu.User)
        .Where(cg => cg.ChatUsers.Any(cu => cu.UserId == userId))
        .ToListAsync();
    }

    public async Task<List<ChatMessage>> GetMessagesForGroupAsync(int userId, int chatGroupId)
    {
      return await _chatContext.ChatMessages
        .Where(cm => cm.ChatGroupId == chatGroupId)
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
  }
}